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
        private readonly IProductService _repositoryProduct;
        private readonly IService<Category> _categoryRepository;
        private readonly IService<Brand> _brandRepository;

        public ProductsController(IProductService repositoryProduct, IService<Category> categoryRepository, IService<Brand> brandRepository, IService<Product> repository)
        {
            _repositoryProduct = repositoryProduct;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _repository = repository;
        }

        // GET: ProductsController
        public async Task<ActionResult> Index()
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
                    await _repositoryProduct.AddAsync(product);
                    await _repositoryProduct.SaveChangesAsync();
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
            return View(await _repositoryProduct.FindAsync(id));
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
                    _repositoryProduct.Update(product);
                    await _repositoryProduct.SaveChangesAsync();
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
            return View(await _repositoryProduct.FindAsync(id));
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Product product)
        {
            try
            {
                _repositoryProduct.Delete(product);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
