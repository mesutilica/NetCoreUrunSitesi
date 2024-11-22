using Core.Entities;
using Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.Models;
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
            var appUser = await _service.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser is null)
            {
                return NotFound("Kullanıcı Datası Bulunamadı! Lütfen Tekrar Giriş Yapın!");
            }
            var model = new UserEditViewModel()
            {
                Email = appUser.Email,
                Name = appUser.Name,
                Password = appUser.Password,
                Phone = appUser.Phone,
                Surname = appUser.Surname
            };
            return View(model);

        }
        [HttpPost, Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> Index(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var appUser = await _service.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
                    if (appUser is not null)
                    {
                        appUser.Name = model.Name;
                        appUser.Surname = model.Surname;
                        appUser.Email = model.Email;
                        appUser.Phone = model.Phone;
                        appUser.Password = model.Password;
                        _service.Update(appUser);
                        var sonuc = await _service.SaveChangesAsync();
                        if (sonuc > 0)
                        {
                            TempData["Message"] = @"<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
                        <strong>Bilgileriniz Güncellenmiştir!</strong>
    <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
    </div>";
                            return RedirectToAction(nameof(Index));
                        }
                        ModelState.AddModelError("", "Kayıt Güncellenemedi!");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(model);
        }
        public IActionResult MyOrders()
        {
            return View();
        }
        public IActionResult MyReviews()
        {
            return View();
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

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel loginViewModel)
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
                        return Redirect(string.IsNullOrEmpty(loginViewModel.ReturnUrl) ? "/" : loginViewModel.ReturnUrl);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(loginViewModel);
        }

        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync(); // Çıkış işlemi
            return RedirectToAction("Index", "Home");
        }

        public IActionResult PasswordRenew()
        {
            return View();
        }
        [HttpPost]
        public IActionResult PasswordRenew(LoginViewModel loginViewModel)
        {
            return View();
        }
    }
}