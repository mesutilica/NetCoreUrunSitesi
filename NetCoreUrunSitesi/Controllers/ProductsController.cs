using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.Models;
using Service.Abstract;

namespace WebAPIUsing.Controllers
{
    public class ProductsController : Controller
    {
        /*private readonly IRepository<Product> _productRepository;

        public ProductsController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }*/
        private readonly IProductService _productRepository;
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(IProductService productRepository, HttpClient httpClient, IHttpClientFactory httpClientFactory)
        {
            _productRepository = productRepository;
            _httpClient = httpClient;
            _apiAdres = "https://localhost:7132/Api";
            _httpClientFactory = httpClientFactory;
        }
        [Route("tum-urunlerimiz")]
        public async Task<IActionResult> Index()
        {
            return View(await _productRepository.GetAllAsync(p => p.IsActive));
        }

        public async Task<IActionResult> Search(string q)
        {
            return View(await _productRepository.GetAllAsync(p => p.Name.Contains(q)));
        }

        public async Task<IActionResult> Detail(int id)
        {
            var model = new ProductDetailViewModel();
            var product = await _productRepository.GetProductByCategoryAndBrandAsync(id);
            model.Product = product;
            model.RelatedProducts = await _productRepository.GetAllAsync(p => p.CategoryId == product.CategoryId && p.Id != id);
            if (model is null)
            {
                return NotFound();
            }
            return View(model);
        }
    }
}
