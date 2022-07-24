using Microsoft.AspNetCore.Mvc;
using Entities;
using BL;
using NetCoreUrunSitesi.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IRepository<Product> _repository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Brand> _brandRepository;

        public ProductsController(IRepository<Product> repository, IRepository<Category> categoryRepository, IRepository<Brand> brandRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
        }

        // GET: ProductsController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _repository.GetAllAsync());
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductsController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Product product, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    product.CreateDate = DateTime.Now;
                    product.Image = await FileHelper.FileLoaderAsync(Image);
                    await _repository.AddAsync(product);
                    await _repository.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
            return View(product);
        }

        // GET: ProductsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
            return View(await _repository.FindAsync(id));
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(Product product, IFormFile? Image, bool resmiSil = false)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (resmiSil == true) product.Image = string.Empty;
                    if (Image != null) product.Image = await FileHelper.FileLoaderAsync(Image);
                    _repository.Update(product);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
            return View(product);
        }

        // GET: ProductsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _repository.FindAsync(id));
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Product product)
        {
            try
            {
                _repository.Delete(product);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
