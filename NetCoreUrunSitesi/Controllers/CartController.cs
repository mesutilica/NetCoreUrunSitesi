﻿using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.ExtensionMethods;
using Service.Abstract;

namespace NetCoreUrunSitesi.Controllers
{
    public class CartController : Controller
    {
        private IProductService repository;

        public CartController(IProductService _repository)
        {
            repository = _repository;
        }
        public IActionResult Index()
        {
            return View(GetCart());
        }

        public IActionResult AddToCart(int ProductId, int quantity = 1)
        {
            var product = repository.Find(ProductId);

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
            var product = repository.Find(ProductId);

            if (product != null)
            {
                var cart = GetCart();
                cart.RemoveProduct(product);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }

        private Cart GetCart()
        {
            return HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
        }
    }
}
