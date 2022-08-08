using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.Services;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize] // Apiye bağlanarak kullanıcı işlemleri yapacağımız controller
    public class APIAppUsersController : Controller
    {
        private readonly AppUsersApiService _appUsersApiService; // kullanıcı işlemleri için yazdığımız özel api servisimiz

        public APIAppUsersController(AppUsersApiService appUsersApiService)
        {
            _appUsersApiService = appUsersApiService; // yukardaki nesneyi DI ile doldurduk
        }

        // GET: APIAppUsersController
        public async Task<IActionResult> IndexAsync()
        {
            return View(await _appUsersApiService.GetAllAppUsers());
        }

        // GET: APIAppUsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: APIAppUsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: APIAppUsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _appUsersApiService.AddAsync(appUser);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(appUser);
        }

        // GET: APIAppUsersController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _appUsersApiService.GetByIdAsync(id));
        }

        // POST: APIAppUsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _appUsersApiService.UpdateAsync(id, appUser);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(appUser);
        }

        // GET: APIAppUsersController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _appUsersApiService.GetByIdAsync(id));
        }

        // POST: APIAppUsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, AppUser appUser)
        {
            try
            {
                await _appUsersApiService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
