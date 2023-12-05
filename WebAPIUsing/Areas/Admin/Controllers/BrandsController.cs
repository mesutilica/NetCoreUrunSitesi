using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIUsing.Utils;
//using Newtonsoft.Json;
using System.Net.Http.Headers;
using Core.Entities;

namespace WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class BrandsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres = "https://localhost:7132/Api/Brands";
        private readonly string _apiTokenAdres = "https://localhost:7132/api/Login/connect/token";

        public BrandsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: APIBrandsController
        public async Task<IActionResult> IndexAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            // 1. yöntem
            //var request = await _httpClient.GetFromJsonAsync<List<Brand>>(_apiAdres);
            // return View(request);
            // 2. Yöntem
            var responseMessage = await _httpClient.GetAsync(_apiAdres);
            if (responseMessage.IsSuccessStatusCode) // api ye yaptığımız isteğin sonucu başarılıysa
            {
                // var jsonData = await responseMessage.Content.ReadAsStringAsync(); // responseMessage içeriğini json olarak okuyoruz
                var result = await responseMessage.Content.ReadFromJsonAsync<List<Brand>>();// JsonConvert.DeserializeObject<List<Brand>>(jsonData);
                return View(result); // Sayfa modelimiz olan appuser listesine çevirdiğimiz modeli view a gönderiyoruz
            }
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Redirect("/admin/logout");
            }
            return View(responseMessage.RequestMessage);
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
                    if (resmiSil == true)
                        entity.Logo = string.Empty;
                    if (Logo != null)
                        entity.Logo = await FileHelper.FileLoaderAsync(Logo);
                    var response = await _httpClient.PutAsJsonAsync($"{_apiAdres}/{id}", entity);
                    if (response.IsSuccessStatusCode)
                        return RedirectToAction(nameof(Index));
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
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "Kayıt Silinemedi!");
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            return View(entity);
        }
    }
}
