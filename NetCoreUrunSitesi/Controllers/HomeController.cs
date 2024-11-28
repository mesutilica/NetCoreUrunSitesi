using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.Models;
using NetCoreUrunSitesi.Utils;
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
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public HomeController(IService<Slider> sliderRepository, IService<News> newsRepository, IProductService productService, IService<Contact> serviceContact, IMapper mapper, IConfiguration configuration)
        {
            _sliderRepository = sliderRepository;
            _newsRepository = newsRepository;
            _productService = productService;
            _serviceContact = serviceContact;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            //var sliders = await _sliderRepository.GetAllAsync();

            var model = new HomePageViewModel()
            {
                Sliders = await _sliderRepository.GetAllAsync(),
                //Products = await _productService.GetAllAsync(p => p.IsActive & p.IsHome),
                Products = _mapper.Map<List<ProductListViewDto>>(await _productService.GetAllAsync(p => p.IsActive & p.IsHome)),
                News = await _newsRepository.GetAllAsync()
            };
            //model.Products = await _productService.GetAllProductsByCacheAsync();
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
        [Route("iletisim")]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("iletisim"), HttpPost]
        public async Task<IActionResult> ContactAsync(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceContact.AddAsync(contact);
                    var sonuc = await _serviceContact.SaveChangesAsync();
                    if (sonuc > 0)
                    {
                        //await MailHelper.SendMailAsync(contact, _configuration); // gelen mesajı mail gönder.
                        TempData["Message"] = "<div class='alert alert-success'>Mesajınız Gönderildi! Teşekkürler..</div>";
                        return RedirectToAction("ContactUs");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            //return View(contact);
            return Problem("Kayıt Başarısız!");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}