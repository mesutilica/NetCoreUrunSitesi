using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreUrunSitesi.Utils;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class APIProductsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;

        public APIProductsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiAdres = "https://localhost:7132/Api/";
        }

        // GET: APIProductsController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres + "Products"));
        }

        // GET: APIProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: APIProductsController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.CategoryId = new SelectList(await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres + "Categories"), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _httpClient.GetFromJsonAsync<List<Brand>>(_apiAdres + "Brands"), "Id", "Name");
            return View();
        }

        // POST: APIProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Product entity, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entity.CreateDate = DateTime.Now;
                    entity.Image = await FileHelper.FileLoaderAsync(Image);
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres + "Products", entity);
                    if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                    ModelState.AddModelError("", "Kayıt Eklenemedi!");
                }
                catch (Exception hata)
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                    ViewData["Err"] = hata.Message;
                }
            }
            ViewBag.CategoryId = new SelectList(await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres + "Categories"), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _httpClient.GetFromJsonAsync<List<Brand>>(_apiAdres + "Brands"), "Id", "Name");
            return View(entity);
        }

        // GET: APIProductsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            ViewBag.CategoryId = new SelectList(await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres + "Categories"), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _httpClient.GetFromJsonAsync<List<Brand>>(_apiAdres + "Brands"), "Id", "Name");
            return View(await _httpClient.GetFromJsonAsync<Product>($"{_apiAdres + "Products"}/{id}"));
        }

        // POST: APIProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Product entity, IFormFile? Image, bool resmiSil = false)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (resmiSil == true) entity.Image = string.Empty;
                    if (Image != null) entity.Image = await FileHelper.FileLoaderAsync(Image);
                    var response = await _httpClient.PutAsJsonAsync($"{_apiAdres + "Products"}/{id}", entity);
                    if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                    ModelState.AddModelError("", "Kayıt Güncellenemedi!");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            ViewBag.CategoryId = new SelectList(await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres + "Categories"), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _httpClient.GetFromJsonAsync<List<Brand>>(_apiAdres + "Brands"), "Id", "Name");
            return View(entity);
        }

        // GET: APIProductsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _httpClient.GetFromJsonAsync<Product>($"{_apiAdres + "Products"}/{id}"));
        }

        // POST: APIProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Product entity)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiAdres + "Products"}/{id}");
                if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "Kayıt Güncellenemedi!");
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            return View(entity);
        }
    }
}
