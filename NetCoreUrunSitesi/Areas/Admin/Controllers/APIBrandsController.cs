using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class APIBrandsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;
        public APIBrandsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiAdres = "http://localhost:5084/api/Brands";
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
        public async Task<ActionResult> CreateAsync(Brand brand)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_apiAdres, brand);

                if (!response.IsSuccessStatusCode) return null;

                //var responseBody = await response.Content.ReadFromJsonAsync<Brand>();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(brand);
            }
        }

        // GET: APIBrandsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(await _httpClient.GetFromJsonAsync<Brand>($"{_apiAdres}/{id}"));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Brand brand)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _httpClient.PutAsJsonAsync($"{_apiAdres}/{id}", brand);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(brand);
        }

        // GET: APIBrandsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: APIBrandsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }
    }
}
