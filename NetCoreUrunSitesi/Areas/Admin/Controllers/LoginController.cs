using Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.Models;
using Service.Abstract;
using System.Security.Claims;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly IService<AppUser> _repository;

        public LoginController(IService<AppUser> repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(AdminLoginViewModel adminLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = _repository.Get(x => x.Username == adminLoginViewModel.UserName & x.Password == adminLoginViewModel.Password & x.IsActive);
                    if (account == null)
                    {
                        ModelState.AddModelError("", "Giriş Başarısız!");
                    }
                    else
                    {
                        var claims = new List<Claim>() // Claim = hak
                        {
                            new(ClaimTypes.Name, account.Username),
                            new(ClaimTypes.Role, account.IsAdmin ? "Admin" : "User"),
                            new("UserId", account.Id.ToString()),
                            new("UserGuid", account.UserGuid.ToString())
                        };
                        var userIdentity = new ClaimsIdentity(claims, "Login");
                        ClaimsPrincipal principal = new(userIdentity);
                        await HttpContext.SignInAsync(principal); // , authProperties
                        return Redirect(string.IsNullOrEmpty(HttpContext.Request.Query["ReturnUrl"]) ? "/Admin" : HttpContext.Request.Query["ReturnUrl"]);
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
            await HttpContext.SignOutAsync(); // Çıkış işlemi
            return RedirectToAction("Index", "Login");
        }
    }
}
