using Microsoft.AspNetCore.Mvc;
using Entities;
using NetCoreUrunSitesi.Utils;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class APIBrandsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;

        public APIBrandsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiAdres = "https://localhost:7132/Api/Brands";
        }

        // GET: APIBrandsController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _httpClient.GetFromJsonAsync<List<Brand>>(_apiAdres));
        }

        // GET: APIBrandsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: APIBrandsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: APIBrandsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Brand brand, IFormFile? Logo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    brand.CreateDate = DateTime.Now;
                    brand.Logo = await FileHelper.FileLoaderAsync(Logo);
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres, brand);
                    if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                    ModelState.AddModelError("", "Kayıt Başarısız!");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(brand);
        }

        // GET: APIBrandsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            return View(await _httpClient.GetFromJsonAsync<Brand>($"{_apiAdres}/{id}"));
        }

        // POST: APIBrandsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Brand entity, IFormFile? Logo, bool resmiSil = false)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (resmiSil == true) entity.Logo = string.Empty;
                    if (Logo != null) entity.Logo = await FileHelper.FileLoaderAsync(Logo);
                    var response = await _httpClient.PutAsJsonAsync($"{_apiAdres}/{id}", entity);
                    if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                    ModelState.AddModelError("", "Güncelleme Başarısız Oldu!");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(entity);
        }

        // GET: APIBrandsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _httpClient.GetFromJsonAsync<Brand>($"{_apiAdres}/{id}"));
        }

        // POST: APIBrandsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Brand entity)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiAdres}/{id}");
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
