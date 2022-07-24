using Microsoft.AspNetCore.Mvc;
using Entities;
using BL;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class AppUsersController : Controller
    {
        //AppUserManager manager = new AppUserManager(); // Klasik kullandığımız yöntem

        private readonly IRepository<AppUser> _repository; // DI-Dependency injection yöntemiyle

        public AppUsersController(IRepository<AppUser> repository)
        {
            _repository = repository;
        }

        // GET: AppUsersController
        public ActionResult Index()
        {
            //return View(manager.GetAll()); el klasiko
            return View(_repository.GetAll());
        }

        // GET: AppUsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AppUsersController/Create
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
                    await _repository.AddAsync(appUser);
                    await _repository.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(appUser);
        }

        // GET: AppUsersController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(await _repository.FindAsync(id));
        }

        // POST: AppUsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Update(appUser);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(appUser);
        }

        // GET: AppUsersController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return View(await _repository.FindAsync(id));
        }

        // POST: AppUsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AppUser appUser)
        {
            try
            {
                _repository.Delete(appUser);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
