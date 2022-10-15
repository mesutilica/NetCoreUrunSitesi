using BL.Abstract;
using Entities;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.Models;
using System.Diagnostics;

namespace NetCoreUrunSitesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Slider> _sliderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IProductService _productService;
        private readonly IRepository<News> _newsRepository;

        public HomeController(IRepository<Slider> sliderRepository, IRepository<Product> productRepository, IRepository<News> newsRepository, IProductService productService)
        {
            _sliderRepository = sliderRepository;
            _productRepository = productRepository;
            _newsRepository = newsRepository;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            //var sliders = await _sliderRepository.GetAllAsync();

            var model = new HomePageViewModel()
            {
                Sliders = _sliderRepository.GetAll(),
                //Products = _productRepository.GetAll(),
                News = _newsRepository.GetAll()
            };
            model.Products = await _productService.GetAllProductsByCacheAsync();
            return View(model);
        }
        [BasicAuthorize]
        public IActionResult Privacy()
        {
            return View();
        }
        [Route("AccesDenied")]
        public IActionResult AccesDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}