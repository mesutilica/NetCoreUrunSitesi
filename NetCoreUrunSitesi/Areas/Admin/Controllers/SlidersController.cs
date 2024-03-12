using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.Utils;
using Service.Abstract;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class SlidersController : Controller
    {
        private readonly IService<Slider> _service;

        public SlidersController(IService<Slider> repository)
        {
            _service = repository;
        }

        // GET: SlidersController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _service.GetAllAsync());
        }

        // GET: SlidersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SlidersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SlidersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Slider slider, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    slider.Image = await FileHelper.FileLoaderAsync(Image);
                    await _service.AddAsync(slider);
                    await _service.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(slider);
        }

        // GET: SlidersController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            return View(await _service.FindAsync(id));
        }

        // POST: SlidersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(Slider slider, IFormFile? Image, bool resmiSil = false)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (resmiSil == true)
                        slider.Image = string.Empty;
                    if (Image != null)
                        slider.Image = await FileHelper.FileLoaderAsync(Image);
                    _service.Update(slider);
                    await _service.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(slider);
        }

        // GET: SlidersController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _service.FindAsync(id));
        }

        // POST: SlidersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Slider slider)
        {
            try
            {
                _service.Delete(slider);
                await _service.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
