using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.ExtensionMethods;
using NetCoreUrunSitesi.Models;
using Service.Abstract;
using Service.Concrete;

namespace NetCoreUrunSitesi.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            var cart = GetCart();
            var model = new CartViewModel()
            {
                CartProducts = cart.Products,
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
                SaveCart(cart);
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
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        private void SaveCart(CartService cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }

        private CartService GetCart()
        {
            return HttpContext.Session.GetJson<CartService>("Cart") ?? new CartService();
        }
    }
}
