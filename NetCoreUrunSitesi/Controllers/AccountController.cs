using Core.Entities;
using Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using System.Security.Claims;

namespace NetCoreUrunSitesi.Controllers
{
    public class AccountController : Controller
    {
        private readonly IService<AppUser> _service;

        public AccountController(IService<AppUser> service)
        {
            _service = service;
        }
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> Index()
        {
            var model = await _service.FirstOrDefaultAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (model is null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost, Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> Index(AppUser appUser)
        {
            try
            {
                _service.Update(appUser);
                await _service.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            return View(appUser);
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    appUser.IsActive = true;
                    appUser.IsAdmin = false;
                    await _service.AddAsync(appUser);
                    await _service.SaveChangesAsync();
                    TempData["Message"] = @$"<div class=""alert alert-success"">Kayıt Başarılı..</div>";
                    return RedirectToAction("Index", "Home");
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
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = _service.Get(x => x.Email == loginViewModel.Email & x.Password == loginViewModel.Password & x.IsActive);
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
                        return Redirect(string.IsNullOrEmpty(HttpContext.Request.Query["ReturnUrl"]) ? "/" : HttpContext.Request.Query["ReturnUrl"]);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View();
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(); // Çıkış işlemi
            return RedirectToAction("Index", "Home");
        }
    }
}