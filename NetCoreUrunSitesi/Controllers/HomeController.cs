using Entities;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.Models;
using Service.Abstract;
using System.Diagnostics;

namespace NetCoreUrunSitesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService<Slider> _sliderRepository;
        private readonly IProductService _productService;
        private readonly IService<News> _newsRepository;
        private readonly IService<Contact> _serviceContact;

        public HomeController(IService<Slider> sliderRepository, IService<News> newsRepository, IProductService productService, IService<Contact> serviceContact)
        {
            _sliderRepository = sliderRepository;
            _newsRepository = newsRepository;
            _productService = productService;
            _serviceContact = serviceContact;
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
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContactAsync(Contact contact)
        {
            if (ModelState.IsValid)
            {
                await _serviceContact.AddAsync(contact);
                await _serviceContact.SaveChangesAsync();
                TempData["mesaj"] = "<div class='alert alert-success'>Mesajınız Gönderilmiştir. Teşekkürler..</div>";
                return RedirectToAction("Contact");
            }
            return View(contact);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}