﻿using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.ExtensionMethods;
using NetCoreUrunSitesi.Models;
using Service.Abstract;
using Service.Concrete;

namespace NetCoreUrunSitesi.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IProductService _productService;
        private readonly CartService _cartService;

        public FavoritesController(IProductService productService, CartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
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

        public IActionResult RemoveFromCart(int ProductId)
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
            HttpContext.Session.SetJson("Favorites", cart);
        }

        private CartService GetCart()
        {
            return HttpContext.Session.GetJson<CartService>("Favorites") ?? new CartService();
        }
    }
}
