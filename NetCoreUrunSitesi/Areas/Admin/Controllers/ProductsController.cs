using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCoreUrunSitesi.Utils;
using Service.Abstract;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class ProductsController : Controller
    {
        private readonly IService<Product> _repository;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IService<Brand> _brandService;

        public ProductsController(IProductService productService, ICategoryService categoryService, IService<Brand> brandService, IService<Product> repository)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _repository = repository;
        }

        // GET: ProductsController
        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetQueryable().Include(c => c.Category).Include(c => c.Brand).ToListAsync());
            //return View(await _repositoryProduct.GetAllProductsByCategoriesBrandsAsync());
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductsController/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.CategoryId = new SelectList(await _categoryService.GetCategoriesByIdNameAsync(), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _brandService.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(Product product, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    product.CreateDate = DateTime.Now;
                    product.Image = await FileHelper.FileLoaderAsync(Image);
                    await _productService.AddAsync(product);
                    await _productService.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            ViewBag.CategoryId = new SelectList(await _categoryService.GetCategoriesByIdNameAsync(), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _brandService.GetAllAsync(), "Id", "Name");
            return View(product);
        }

        // GET: ProductsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            ViewBag.CategoryId = new SelectList(await _categoryService.GetCategoriesByIdNameAsync(), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _brandService.GetAllAsync(), "Id", "Name");
            return View(await _productService.FindAsync(id));
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
                    _productService.Update(product);
                    await _productService.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            ViewBag.CategoryId = new SelectList(await _categoryService.GetCategoriesByIdNameAsync(), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _brandService.GetAllAsync(), "Id", "Name");
            return View(product);
        }

        // GET: ProductsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _productService.FindAsync(id));
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Product product)
        {
            try
            {
                _productService.Delete(product);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
