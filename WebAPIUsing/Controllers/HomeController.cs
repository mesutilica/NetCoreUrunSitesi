using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAPIUsing.Models;

namespace WebAPIUsing.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string _apiAdres = "https://localhost:7132/api/";

        public async Task<IActionResult> Index()
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres + "Products");
            var model = new HomePageViewModel()
            {
                Sliders = await _httpClient.GetFromJsonAsync<List<Slider>>(_apiAdres + "Sliders"),
                Products = products.Where(p => p.IsActive && p.IsHome).ToList(),
                News = await _httpClient.GetFromJsonAsync<List<News>>(_apiAdres + "News")
            };
            return View(model);
        }
        //[BasicAuthorize]
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
        public IActionResult ContactUs()
        {
            return View();
        }

        [Route("iletisim"), HttpPost]
        public async Task<IActionResult> ContactUs(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres + "Contacts", contact);
                    if (response.IsSuccessStatusCode)
                    {
                        // await MailHelper.SendMailAsync(contact); // gelen mesajı mail gönder.
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