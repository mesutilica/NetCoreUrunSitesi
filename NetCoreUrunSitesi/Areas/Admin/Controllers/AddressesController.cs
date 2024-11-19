using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Abstract;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class AddressesController : Controller
    {
        private readonly IService<AppUser> _serviceAppUser;
        private readonly IService<Address> _serviceAddress;

        public AddressesController(IService<AppUser> serviceAppUser, IService<Address> serviceAddress)
        {
            _serviceAppUser = serviceAppUser;
            _serviceAddress = serviceAddress;
        }

        // GET: AddressesController
        public ActionResult Index(int? appUserId)
        {
            var model = _serviceAddress.GetQueryable();
            if (appUserId != null)
            {
                model.Where(u => u.AppUserId == appUserId);
            }
            return View(model);
        }

        // GET: AddressesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AddressesController/Create
        public ActionResult Create(int? appUserId)
        {
            ViewBag.AppUserId = new SelectList(_serviceAppUser.GetQueryable(), "Id", "Name");
            return View();
        }

        // POST: AddressesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Address collection)
        {
            try
            {
                _serviceAddress.Add(collection);
                _serviceAddress.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            ViewBag.AppUserId = new SelectList(_serviceAppUser.GetQueryable(), "Id", "Name");
            return View(collection);
        }

        // GET: AddressesController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.AppUserId = new SelectList(_serviceAppUser.GetQueryable(), "Id", "Name");
            var model = _serviceAddress.Find(id);
            return View(model);
        }

        // POST: AddressesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Address collection)
        {
            try
            {
                _serviceAddress.Update(collection);
                _serviceAddress.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            ViewBag.AppUserId = new SelectList(_serviceAppUser.GetQueryable(), "Id", "Name");
            return View(collection);
        }

        // GET: AddressesController/Delete/5
        public ActionResult Delete(int id)
        {
            var model = _serviceAddress.Find(id);
            return View(model);
        }

        // POST: AddressesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Address collection)
        {
            try
            {
                _serviceAddress.Delete(collection);
                _serviceAddress.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            return View(collection);
        }
    }
}
