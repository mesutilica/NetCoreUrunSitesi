using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.Utils;
using Service.Abstract;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class BrandsController : Controller
    {
        private readonly IService<Brand> _repository;

        public BrandsController(IService<Brand> repository)
        {
            _repository = repository;
        }

        // GET: BrandsController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _repository.GetAllAsync());
        }

        // GET: BrandsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BrandsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BrandsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Brand brand, IFormFile? Logo) // .net core da resim yükleme
        {
            if (ModelState.IsValid)
            {
                try
                {
                    brand.CreateDate = DateTime.Now;
                    brand.Logo = await FileHelper.FileLoaderAsync(Logo);
                    await _repository.AddAsync(brand);
                    await _repository.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(brand);
        }

        // GET: BrandsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            return View(await _repository.FindAsync(id));
        }

        // POST: BrandsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(Brand brand, IFormFile? Logo, bool resmiSil = false)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (resmiSil == true) brand.Logo = string.Empty;
                    if (Logo != null) brand.Logo = await FileHelper.FileLoaderAsync(Logo);
                    _repository.Update(brand);
                    await _repository.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(brand);
        }

        // GET: BrandsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _repository.FindAsync(id));
        }

        // POST: BrandsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Brand brand)
        {
            try
            {
                _repository.Delete(brand);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
