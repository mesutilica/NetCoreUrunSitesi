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
                    var account = _repository.Get(x => x.Username == adminLoginViewModel.UserName & x.Password == adminLoginViewModel.Password & x.IsActive);
                    if (account == null)
                    {
                        ModelState.AddModelError("", "Giriş Başarısız!");
                    }
                    else
                    {
                        var claims = new List<Claim>() // Claim = hak
                        {
                            new Claim(ClaimTypes.Name, account.Username),
                            new Claim(ClaimTypes.Role, account.IsAdmin ? "Admin" : "User"),
                            new Claim("UserId", account.Id.ToString()),
                            new Claim("UserGuid", account.UserGuid.ToString())
                        };
                        var userIdentity = new ClaimsIdentity(claims, "Login");
                        /*var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            ExpiresUtc = DateTime.UtcNow.AddDays(7),
                            IsPersistent = true,
                            RedirectUri = "https://localhost:7113/Admin/Logout"
                        };*/
                        ClaimsPrincipal principal = new(userIdentity);
                        await HttpContext.SignInAsync(principal); // , authProperties
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
            await HttpContext.SignOutAsync(); // Çıkış işlemi
            return RedirectToAction("Index", "Login");
        }
    }
}
