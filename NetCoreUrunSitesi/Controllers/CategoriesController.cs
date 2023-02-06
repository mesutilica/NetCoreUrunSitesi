using Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace WebAPIUsing.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryRepository;
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;

        public CategoriesController(ICategoryService categoryRepository, HttpClient httpClient)
        {
            _categoryRepository = categoryRepository;
            _httpClient = httpClient;
            _apiAdres = "https://localhost:7132/Api/Categories";
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null) return BadRequest();

            //var kategori = _categoryRepository.Find(id);//_repositoryCategory.Find(id);

            //var kat = await _categoryRepository.GetCategoryByProductsAsync(id.Value);

            // API den çekme
            var katApi = await _httpClient.GetFromJsonAsync<Category>($"{_apiAdres}/GetCategoryByProducts/{id}");

            return View(katApi); //kat
        }
    }
}
