using Core.Entities;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
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
        private readonly IService<Core.Entities.Address> _serviceAddress;
        private readonly IService<Order> _serviceOrder;
        private readonly IConfiguration _configuration;
        public CartController(IService<Product> productService, IService<AppUser> service, IService<Core.Entities.Address> serviceAddress, IService<Order> serviceOrder, IConfiguration configuration)
        {
            _productService = productService;
            _service = service;
            _serviceAddress = serviceAddress;
            _serviceOrder = serviceOrder;
            _configuration = configuration;
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
        public async Task<IActionResult> CheckoutAsync(string CardNameSurname, string CardNumber, string CardMonth, string CardYear, string CVV, string DeliveryAddress, string BillingAddress)
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
                BillingAddress = $"{faturaAdresi.OpenAddress} {faturaAdresi.District} {faturaAdresi.City}",//BillingAddress,
                DeliveryAddress = $"{teslimatAdresi.OpenAddress} {teslimatAdresi.District} {teslimatAdresi.City}",//DeliveryAddress,
                OrderDate = DateTime.Now,
                OrderNumber = Guid.NewGuid().ToString(),
                TotalPrice = cart.TotalPrice(),
                OrderState = 0,
                OrderLines = []
            };

            #region Iyzico
            Options options = new Options();
            options.ApiKey = _configuration["IyzicOptions:ApiKey"];
            options.SecretKey = _configuration["IyzicOptions:SecretKey"];
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = HttpContext.Session.Id;
            request.Price = siparis.TotalPrice.ToString().Replace(",", ".");
            request.PaidPrice = siparis.TotalPrice.ToString().Replace(",", ".");
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B" + HttpContext.Session.Id;
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = CardNameSurname; // "John Doe";
            paymentCard.CardNumber = CardNumber; // "5528790000000008";
            paymentCard.ExpireMonth = CardMonth; // "12";
            paymentCard.ExpireYear = CardYear; // "2030";
            paymentCard.Cvc = CVV; // "123";
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY" + appUser.Id;
            buyer.Name = appUser.Name;
            buyer.Surname = appUser.Surname;
            buyer.GsmNumber = appUser.Phone;
            buyer.Email = appUser.Email;
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = DateTime.Now.ToString();
            buyer.RegistrationDate = appUser.CreateDate.ToString();
            buyer.RegistrationAddress = siparis.DeliveryAddress;
            buyer.Ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            var shippingAddress = new Iyzipay.Model.Address();
            shippingAddress.ContactName = appUser.Name + " " + appUser.Surname;
            shippingAddress.City = teslimatAdresi.City;
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = teslimatAdresi.OpenAddress;
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            var billingAddress = new Iyzipay.Model.Address();
            billingAddress.ContactName = appUser.Name + " " + appUser.Surname;
            billingAddress.City = faturaAdresi.City;
            billingAddress.Country = "Turkey";
            billingAddress.Description = faturaAdresi.OpenAddress;
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            var basketItems = new List<BasketItem>();

            foreach (var item in cart.CartLines)
            {
                siparis.OrderLines.Add(new OrderLine
                {
                    ProductId = item.Product.Id,
                    OrderId = siparis.Id,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                });
                basketItems.Add(new BasketItem
                {
                    Id = item.Product.Id.ToString(),
                    Name = item.Product.Name,
                    Category1 = item.Product.Category?.Name,
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = item.Product.Price.ToString().Replace(",", ".")
                });
            }

            request.BasketItems = basketItems;

            #endregion

            try
            {
                Payment payment = await Payment.Create(request, options);
                if (payment.PaymentStatus == "success")
                {
                    await _serviceOrder.AddAsync(siparis);
                    var sonuc = await _serviceOrder.SaveChangesAsync();
                    if (sonuc > 0)
                    {
                        HttpContext.Session.Remove("Cart");
                        return RedirectToAction("Thanks");
                    }
                }
                else
                {
                    TempData["Message"] = "Ödeme Alınamadı!";
                }
            }
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
