using Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.ExtensionMethods;
using NetCoreUrunSitesi.Models;
using Service.Abstract;
using Service.Concrete;

namespace NetCoreUrunSitesi.Controllers
{
    public class CartController : Controller
    {
        private readonly IService<Product> _productService;
        private readonly IService<AppUser> _service;
        private readonly IService<Address> _serviceAddress;
        private readonly IService<Order> _serviceOrder;
        public CartController(IService<Product> productService, IService<AppUser> service, IService<Address> serviceAddress, IService<Order> serviceOrder)
        {
            _productService = productService;
            _service = service;
            _serviceAddress = serviceAddress;
            _serviceOrder = serviceOrder;
        }
        public IActionResult Index()
        {
            var cart = GetCart();
            var model = new CartViewModel()
            {
                CartProducts = cart.CartLines,
                TotalPrice = cart.TotalPrice()
            };
            return View(model);
        }

        public IActionResult Add(int ProductId, int quantity = 1)
        {
            var product = _productService.Find(ProductId);

            if (product != null)
            {
                var cart = GetCart();
                cart.AddProduct(product, quantity);
                HttpContext.Session.SetJson("Cart", cart);
                return Redirect(Request.Headers["Referer"].ToString());
            }

            return RedirectToAction("Index");
        }

        public IActionResult Update(int ProductId, int quantity = 1)
        {
            var product = _productService.Find(ProductId);

            if (product != null)
            {
                var cart = GetCart();
                cart.UpdateProduct(product, quantity);
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int ProductId)
        {
            var product = _productService.Find(ProductId);

            if (product != null)
            {
                var cart = GetCart();
                cart.RemoveProduct(product);
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        private CartService GetCart()
        {
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{

            //}
            return HttpContext.Session.GetJson<CartService>("Cart") ?? new CartService();
        }
        [Authorize]
        public async Task<IActionResult> CheckoutAsync()
        {
            var cart = GetCart();

            var appUser = await _service.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("SignIn", "Account");
            }
            var addresses = _serviceAddress.GetAll(a => a.AppUserId == appUser.Id & a.IsActive);
            var model = new CheckoutViewModel()
            {
                CartProducts = cart.CartLines,
                TotalPrice = cart.TotalPrice(),
                Addresses = addresses
            };
            return View(model);
        }
        [Authorize, HttpPost]
        public async Task<IActionResult> CheckoutAsync(string CardMonth, string CardYear, string CVV, string DeliveryAddress, string BillingAddress)
        {
            var cart = GetCart();
            var appUser = await _service.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("SignIn", "Account");
            }
            var addresses = _serviceAddress.GetAll(a => a.AppUserId == appUser.Id & a.IsActive);
            var model = new CheckoutViewModel()
            {
                CartProducts = cart.CartLines,
                TotalPrice = cart.TotalPrice(),
                Addresses = addresses
            };
            if (string.IsNullOrWhiteSpace(CardMonth) || string.IsNullOrWhiteSpace(CardYear) || string.IsNullOrWhiteSpace(CVV) || string.IsNullOrWhiteSpace(DeliveryAddress) || string.IsNullOrWhiteSpace(BillingAddress))
            {
                return View(model);
            }
            var teslimatAdresi = addresses.FirstOrDefault(a => a.AddressGuid.ToString() == DeliveryAddress);
            var faturaAdresi = addresses.FirstOrDefault(a => a.AddressGuid.ToString() == BillingAddress);

            var siparis = new Order
            {
                AppUserId = appUser.Id,
                CustomerId = appUser.UserGuid.ToString(),
                BillingAddress = BillingAddress,
                DeliveryAddress = DeliveryAddress,
                OrderDate = DateTime.Now,
                OrderNumber = Guid.NewGuid().ToString(),
                TotalPrice = cart.TotalPrice(),
                OrderState = 0,
                OrderLines = []
            };

            //var siparisDetaylari = new List<OrderLine>();

            foreach (var item in cart.CartLines)
            {
                siparis.OrderLines.Add(new OrderLine
                {
                    ProductId = item.Product.Id,
                    OrderId = siparis.Id,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                });
            }
            // siparis.OrderLines.AddRange(siparisDetaylari);

            await _serviceOrder.AddAsync(siparis);
            var sonuc = await _serviceOrder.SaveChangesAsync();
            if (sonuc > 0)
            {
                HttpContext.Session.Remove("Cart");
                return RedirectToAction("Thanks");
            }
            try
            { }
            catch (Exception)
            {
                TempData["Message"] = "Hata Oluştu!";
            }

            return View(model);
        }
        public IActionResult Thanks()
        {
            return View();
        }
    }
}
