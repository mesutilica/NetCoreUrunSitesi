﻿using Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Abstract;

namespace NetCoreUrunSitesi.Controllers
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

            //var kat = await _categoryRepository.GetCategoryWithProductsByCategoryIdAsync(id.Value);

            var katApi = await _httpClient.GetFromJsonAsync<Category>($"{_apiAdres}/GetCategoryByProducts/{id}");

            //var resp = await _httpClient.GetAsync($"{_apiAdres}/GetCategoryByProducts/{id}");
            //var respBody = await resp.Content.ReadAsStringAsync();
            //respBody = respBody.Trim('\"').Replace("\\", "");
            //var listOfInstances = System.Text.Json.JsonSerializer.Deserialize<Category>(respBody);
            //var category = JsonConvert.DeserializeObject<Category>(respBody);
            return View(katApi);
        }
    }
}
