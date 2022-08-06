using Microsoft.AspNetCore.Mvc;
using Entities;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class APINewsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;

        public APINewsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiAdres = "https://localhost:7132/Api/News";
        }

        // GET: APINewsController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _httpClient.GetFromJsonAsync<List<News>>(_apiAdres));
        }

        // GET: APINewsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: APINewsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: APINewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(News entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entity.CreateDate = DateTime.Now;
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres, entity);
                    if (!response.IsSuccessStatusCode) return null;
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(entity);
        }

        // GET: APINewsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            return View(await _httpClient.GetFromJsonAsync<News>($"{_apiAdres}/{id}"));
        }

        // POST: APINewsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, News entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
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

        // GET: APINewsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _httpClient.GetFromJsonAsync<News>($"{_apiAdres}/{id}"));
        }

        // POST: APINewsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, News entity)
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
