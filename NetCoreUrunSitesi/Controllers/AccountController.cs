using Entities;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreUrunSitesi.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;

        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiAdres = "https://localhost:7132/Api/";
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    appUser.CreateDate = DateTime.Now;
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres + "AppUsers", appUser);
                    if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                    ModelState.AddModelError("", "Kayıt Başarısız!");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(appUser);
        }
    }
}
