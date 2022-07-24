using BL;
using Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.Models;
using System.Security.Claims;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly IRepository<AppUser> _repository;

        public LoginController(IRepository<AppUser> repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IndexAsync(AdminLoginViewModel adminLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = _repository.Get(x => x.Username == adminLoginViewModel.UserName & x.Password == adminLoginViewModel.Password & x.IsActive & x.IsAdmin);
                    if (account == null)
                    {
                        ModelState.AddModelError("", "Giriş Başarısız!");
                    }
                    else
                    {
                        var claims = new List<Claim>() // Claim = hak
                        {
                            new Claim(ClaimTypes.Name, account.Username)
                        };
                        var userIdentity = new ClaimsIdentity(claims, "Login");
                        ClaimsPrincipal principal = new(userIdentity);
                        await HttpContext.SignInAsync(principal);
                        return Redirect("/Admin/Home");
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
