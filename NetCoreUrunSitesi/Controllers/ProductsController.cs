﻿using BL.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreUrunSitesi.Controllers
{
    public class ProductsController : Controller
    {
        /*private readonly IRepository<Product> _productRepository;

        public ProductsController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }*/
        private readonly IProductService _productRepository;

        public ProductsController(IProductService productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> IndexAsync(int id)
        {
            return View(await _productRepository.GetAllAsync(p => p.CategoryId == id));
        }
    }
}
