using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIUsing.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;
        public CategoriesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiAdres = "https://localhost:7132/Api/Categories";
        }
        public async Task<IActionResult> IndexAsync(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Category>($"{_apiAdres}/GetCategoryByProducts/{id}");
            return View(model);
        }
    }
}
