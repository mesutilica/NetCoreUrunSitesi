using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

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

        public async Task<IActionResult> Index(int id)
        {
            return View(await _productRepository.GetAllAsync(p => p.CategoryId == id));
        }

        public async Task<IActionResult> Search(string q)
        {
            return View(await _productRepository.GetAllAsync(p => p.Name.Contains(q)));
        }

        public async Task<IActionResult> Detail(int id)
        {
            return View(await _productRepository.FindAsync(id));
        }
    }
}
