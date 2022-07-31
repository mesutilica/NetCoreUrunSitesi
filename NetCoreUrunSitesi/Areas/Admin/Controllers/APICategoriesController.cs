using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class APICategoriesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;

        public APICategoriesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiAdres = "https://localhost:7132/Api/Categories";
        }


        // GET: APICategoriesController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres));
        }

        // GET: APICategoriesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: APICategoriesController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.ParentId = new SelectList(await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres), "Id", "Name");
            return View();
        }

        // POST: APICategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    category.CreateDate = DateTime.Now;
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres, category);
                    if (!response.IsSuccessStatusCode) return null;
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            ViewBag.ParentId = new SelectList(await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres), "Id", "Name");
            return View(category);
        }

        // GET: APICategoriesController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            ViewBag.ParentId = new SelectList(await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres), "Id", "Name");
            return View(await _httpClient.GetFromJsonAsync<Category>($"{_apiAdres}/{id}"));
        }

        // POST: APICategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Category entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _httpClient.PutAsJsonAsync($"{_apiAdres}/{id}", entity);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            ViewBag.ParentId = new SelectList(await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres), "Id", "Name");
            return View(entity);
        }

        // GET: APICategoriesController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _httpClient.GetFromJsonAsync<Category>($"{_apiAdres}/{id}"));
        }

        // POST: APICategoriesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Category entity)
        {
            try
            {
                await _httpClient.DeleteAsync($"{_apiAdres}/{id}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
