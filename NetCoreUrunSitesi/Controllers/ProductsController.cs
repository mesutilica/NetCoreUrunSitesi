using BL;
using BL.Abstract;
using Entities;
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
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> IndexAsync(int id)
        {
            return View(await _productRepository.GetAllAsync(p => p.CategoryId == id));
        }
    }
}
