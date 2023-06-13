using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIUsing.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;
        private static readonly string Username = "test";
        private static readonly string Password = "test@123";
        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiAdres = "https://localhost:7132/Api/";
        }

        public async Task<IActionResult> IndexAsync()
        {
            return View();
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //_httpClient.DefaultRequestHeaders.Authorization.Parameter.Insert(0,"test");
                    appUser.CreateDate = DateTime.Now;
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres + "AppUsers", appUser);
                    if (response.IsSuccessStatusCode) return RedirectToAction(nameof(IndexAsync));
                    ModelState.AddModelError("", "Kayıt Başarısız!");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(appUser);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(AppUser appUser)
        {
            return View();
        }
    }
}