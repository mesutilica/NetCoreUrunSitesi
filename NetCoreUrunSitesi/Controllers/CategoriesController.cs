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
            if (id == null)
                return BadRequest("Geçersiz İstek!");
            //var kategori = _categoryRepository.Find(id);//_repositoryCategory.Find(id);
            var model = await _categoryRepository.GetCategoryByProductsAsync(id.Value);
            return View(model); //kat
        }
    }
}
