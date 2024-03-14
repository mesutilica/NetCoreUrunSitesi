using Core.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class AppUsersController : Controller
    {
        //AppUserManager manager = new AppUserManager(); // Klasik kullandığımız yöntem

        private readonly IService<AppUser> _service; // DI-Dependency injection yöntemiyle
        private IValidator<AppUser> _validator;

        public AppUsersController(IService<AppUser> service, IValidator<AppUser> validator)
        {
            _service = service;
            _validator = validator;
        }

        // GET: AppUsersController
        public ActionResult Index()
        {
            //return View(manager.GetAll()); el klasiko
            return View(_service.GetAll());
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
        public async Task<ActionResult> Create(AppUser appUser)
        {
            ValidationResult result = await _validator.ValidateAsync(appUser);
            if (result.IsValid)
            {
                try
                {
                    await _service.AddAsync(appUser);
                    await _service.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.Remove(error.PropertyName);
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage); // FluentValidation hatalarını eklemek için
                    ModelState.AddModelError("", error.ErrorMessage); // hataları üst kısımda göstermek için
                }
            }
            return View();//appUser
        }

        // GET: AppUsersController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(await _service.FindAsync(id));
        }

        // POST: AppUsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, AppUser appUser)
        {
            if (ModelState.IsValid)
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
            }
            return View(appUser);
        }

        // GET: AppUsersController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return View(await _service.FindAsync(id));
        }

        // POST: AppUsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AppUser appUser)
        {
            try
            {
                _service.Delete(appUser);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
