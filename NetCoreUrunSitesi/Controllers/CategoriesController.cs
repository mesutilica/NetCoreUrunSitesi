using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace NetCoreUrunSitesi.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryRepository;
        public CategoriesController(ICategoryService categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null) return BadRequest();

            //var kategori = _categoryRepository.Find(id);//_repositoryCategory.Find(id);

            var kat = await _categoryRepository.GetCategoryWithProductsByCategoryIdAsync(id.Value);

            return View(kat);
        }
    }
}
