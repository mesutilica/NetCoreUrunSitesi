using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreUrunSitesi.Utils;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class APIPostsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;
        private readonly string _kategoriApiAdres;

        public APIPostsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiAdres = "https://localhost:7132/Api/Posts";
            _kategoriApiAdres = "https://localhost:7132/Api/Categories";
        }
        // GET: APIPostsController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _httpClient.GetFromJsonAsync<List<Post>>(_apiAdres));
        }

        // GET: APIPostsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: APIPostsController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.CategoryId = new SelectList(await _httpClient.GetFromJsonAsync<List<Category>>(_kategoriApiAdres), "Id", "Name");
            return View();
        }

        // POST: APIPostsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Post entity, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entity.CreateDate = DateTime.Now;
                    entity.Image = await FileHelper.FileLoaderAsync(Image);
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres, entity);
                    if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                    ModelState.AddModelError("", "Kayıt Eklenemedi!");
                }
                catch (Exception hata)
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                    ViewData["Err"] = hata.Message;
                }
            }
            ViewBag.CategoryId = new SelectList(await _httpClient.GetFromJsonAsync<List<Category>>(_kategoriApiAdres), "Id", "Name");
            return View(entity);
        }

        // GET: APIPostsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            ViewBag.CategoryId = new SelectList(await _httpClient.GetFromJsonAsync<List<Category>>(_kategoriApiAdres), "Id", "Name");
            return View(await _httpClient.GetFromJsonAsync<Post>($"{_apiAdres}/{id}"));
        }

        // POST: APIPostsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Post post, IFormFile? Image, bool resmiSil = false)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (resmiSil == true) post.Image = string.Empty;
                    if (Image != null) post.Image = await FileHelper.FileLoaderAsync(Image);
                    var response = await _httpClient.PutAsJsonAsync($"{_apiAdres}/{id}", post);
                    if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                    ModelState.AddModelError("", "Kayıt Güncellenemedi!");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            ViewBag.CategoryId = new SelectList(await _httpClient.GetFromJsonAsync<List<Category>>(_kategoriApiAdres), "Id", "Name");
            return View(post);
        }

        // GET: APIPostsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _httpClient.GetFromJsonAsync<Post>($"{_apiAdres}/{id}"));
        }

        // POST: APIPostsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Post entity)
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
