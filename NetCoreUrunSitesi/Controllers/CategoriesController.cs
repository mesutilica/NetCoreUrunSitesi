using BL.Abstract;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreUrunSitesi.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IRepository<Category> _repositoryCategory;
        private readonly ICategoryService _categoryRepository;
        public CategoriesController(IRepository<Category> repositoryCategory, ICategoryService categoryRepository)
        {
            _repositoryCategory = repositoryCategory;
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
