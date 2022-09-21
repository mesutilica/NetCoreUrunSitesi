using Microsoft.AspNetCore.Mvc;
using Entities;
using NetCoreUrunSitesi.Utils;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using NetCoreUrunSitesi.Models;
using System.Drawing.Drawing2D;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class APIBrandsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;
        private readonly string _apiTokenAdres;

        public APIBrandsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiAdres = "https://localhost:7132/Api/Brands";
            _apiTokenAdres = "https://localhost:7132/api/Login/connect/token";
        }

        // GET: APIBrandsController
        public async Task<IActionResult> IndexAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NjMzODkxOTAsImV4cCI6MTY2MzM5MDA5MCwiaXNzIjoid3d3LnRlc3QuY29tIiwiYXVkIjoid3d3LnRlc3QuY29tIn0.ok1IxbONOUcpvp-OqtBoJL2cJlqyVeOW4RtCyoTCgFI");
            var request = await _httpClient.GetFromJsonAsync<List<Brand>>(_apiAdres);
            // 2. Yöntem
            var responseMessage = await _httpClient.GetAsync(_apiAdres);
            if (responseMessage.IsSuccessStatusCode) // api ye yaptığımız isteğin sonucu başarılıysa
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync(); // responseMessage içeriğini json olarak okuyoruz
                var result = JsonConvert.DeserializeObject<List<Brand>>(jsonData);
                return View(result); // Sayfa modelimiz olan appuser listesine çevirdiğimiz modeli view a gönderiyoruz
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var tokenModel = new CreateTokenModel()
                {
                    Email = "admin@admin.coo",
                    Password = "123456"
                };
                var response = await _httpClient.PostAsJsonAsync(_apiTokenAdres, tokenModel);
                if (response.IsSuccessStatusCode)
                {
                    var tokenReques = await _httpClient.GetAsync(_apiTokenAdres);
                }

                return View(null);
            }
            return View(request);
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
