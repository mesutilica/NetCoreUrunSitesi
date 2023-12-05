using Azure;
using Core.Entities;
using Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Claims;
using WebAPIUsing.Models;

namespace WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres = "https://localhost:7132/Api/Auth/Login"; // AppUsers

        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult Index(string ReturnUrl)
        {
            var model = new AdminLoginViewModel();
            model.ReturnUrl = ReturnUrl;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> IndexAsync(AdminLoginViewModel adminLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    /*var userList = await _httpClient.GetFromJsonAsync<List<AppUser>>(_apiAdres);
                    var account = userList.FirstOrDefault(x => x.Username == adminLoginViewModel.UserName & x.Password == adminLoginViewModel.Password & x.IsActive);
                    */
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres, adminLoginViewModel);
                    //string stringJWT = await response.Content.ReadAsStringAsync(); // 
                    Token jwt = await response.Content.ReadFromJsonAsync<Token>(); // JsonConvert.DeserializeObject<Token>(stringJWT);
                    if (!response.IsSuccessStatusCode) //  == null
                    {
                        ModelState.AddModelError("", "Giriş Başarısız!");
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", jwt.AccessToken);
                        var claims = new List<Claim>() // Claim = hak
                        { 
                            new(ClaimTypes.Name, "Admin"),
                            new(ClaimTypes.Email, adminLoginViewModel.Email),
                            //new(ClaimTypes.Role, account.IsAdmin ? "Admin" : "User"),
                            //new("UserId", account.Id.ToString()),
                            //new("UserGuid", account.UserGuid.ToString())
                            new("RefreshToken", jwt.RefreshToken),
                            new("Expiration", jwt.Expiration.ToString())
                        };
                        var userIdentity = new ClaimsIdentity(claims, "Login");
                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            ExpiresUtc = DateTime.UtcNow.AddDays(7),
                            IsPersistent = true
                        };
                        ClaimsPrincipal principal = new(userIdentity);
                        await HttpContext.SignInAsync(principal, authProperties);
                        return Redirect(string.IsNullOrEmpty(adminLoginViewModel.ReturnUrl) ? "/Admin" : adminLoginViewModel.ReturnUrl);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(adminLoginViewModel);
        }

        [Route("Admin/Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            HttpContext.Session.Remove("token");
            await HttpContext.SignOutAsync(); // Çıkış işlemi
            return RedirectToAction("Index", "Login");
        }
    }
}
