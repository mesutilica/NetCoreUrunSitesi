using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.Services;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class APIAppUsersController : Controller
    {
        private readonly AppUsersApiService _appUsersApiService;

        public APIAppUsersController(AppUsersApiService appUsersApiService)
        {
            _appUsersApiService = appUsersApiService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _appUsersApiService.GetAllAppUsers());
        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppUsersController/Create
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
        public async Task<ActionResult> Edit(int id)
        {
            return View(await _appUsersApiService.GetByIdAsync(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _appUsersApiService.UpdateAsync(appUser);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(appUser);
        }
        public async Task<ActionResult> Delete(int id)
        {
            return View(await _appUsersApiService.GetByIdAsync(id));
        }

        // POST: AppUsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, AppUser appUser)
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
