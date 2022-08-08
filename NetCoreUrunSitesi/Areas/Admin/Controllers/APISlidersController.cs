using Microsoft.AspNetCore.Mvc;
using Entities;
using NetCoreUrunSitesi.Utils;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class APISlidersController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;

        public APISlidersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiAdres = "https://localhost:7132/Api/Sliders";
        }
        // GET: APISlidersController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _httpClient.GetFromJsonAsync<List<Slider>>(_apiAdres));
        }

        // GET: APISlidersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: APISlidersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: APISlidersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Slider entity, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entity.Image = await FileHelper.FileLoaderAsync(Image);
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres, entity);
                    if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(entity);
        }

        // GET: APISlidersController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            return View(await _httpClient.GetFromJsonAsync<Slider>($"{_apiAdres}/{id}"));
        }

        // POST: APISlidersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Slider entity, IFormFile? Image, bool resmiSil = false)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (resmiSil == true) entity.Image = string.Empty;
                    if (Image != null) entity.Image = await FileHelper.FileLoaderAsync(Image);
                    var response = await _httpClient.PutAsJsonAsync($"{_apiAdres}/{id}", entity);
                    if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                    ModelState.AddModelError("", "Kayıt Güncellenemedi!");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(entity);
        }

        // GET: APISlidersController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _httpClient.GetFromJsonAsync<Slider>($"{_apiAdres}/{id}"));
        }

        // POST: APISlidersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Slider entity)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiAdres}/{id}");
                if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "İşlem Başarısız!");
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            return View(entity);
        }
    }
}
