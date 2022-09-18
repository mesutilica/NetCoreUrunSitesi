using BL;
using BL.Abstract;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreUrunSitesi.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IRepository<Category> _repositoryCategory;
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesController(IRepository<Category> repositoryCategory, ICategoryRepository categoryRepository)
        {
            _repositoryCategory = repositoryCategory;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> IndexAsync(int id)
        {
            var kategori = _categoryRepository.Find(id);//_repositoryCategory.Find(id);

            var kat = await _categoryRepository.GetCategoryWithProductsByCategoryIdAsync(kategori.Id);

            return View(kat);
        }
    }
}
