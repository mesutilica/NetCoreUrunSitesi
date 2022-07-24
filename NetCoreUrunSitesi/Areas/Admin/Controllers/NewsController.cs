using Microsoft.AspNetCore.Mvc;
using Entities;
using BL;
using NetCoreUrunSitesi.Utils;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class NewsController : Controller
    {
        private readonly IRepository<News> _repository;

        public NewsController(IRepository<News> repository)
        {
            _repository = repository;
        }

        // GET: NewsController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _repository.GetAllAsync());
        }

        // GET: NewsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NewsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(News news, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    news.CreateDate = DateTime.Now;
                    news.Image = await FileHelper.FileLoaderAsync(Image);
                    await _repository.AddAsync(news);
                    await _repository.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(news);
        }

        // GET: NewsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            return View(await _repository.FindAsync(id));
        }

        // POST: NewsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(News news, IFormFile? Image, bool resmiSil = false)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (resmiSil == true) news.Image = string.Empty;
                    if (Image != null) news.Image = await FileHelper.FileLoaderAsync(Image);
                    _repository.Update(news);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(news);
        }

        // GET: NewsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _repository.FindAsync(id));
        }

        // POST: NewsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, News news)
        {
            try
            {
                _repository.Delete(news);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
