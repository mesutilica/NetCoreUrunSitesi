using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class APIContactsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;

        public APIContactsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiAdres = "https://localhost:7132/Api/Contacts";
        }

        // GET: APIContactsController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _httpClient.GetFromJsonAsync<List<Contact>>(_apiAdres));
        }

        // GET: APIContactsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: APIContactsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: APIContactsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    contact.CreateDate = DateTime.Now;
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres, contact);
                    if (!response.IsSuccessStatusCode) return null;
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(contact);
        }

        // GET: APIContactsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            return View(await _httpClient.GetFromJsonAsync<Contact>($"{_apiAdres}/{id}"));
        }

        // POST: APIContactsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Contact entity)
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

        // GET: APIContactsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _httpClient.GetFromJsonAsync<Contact>($"{_apiAdres}/{id}"));
        }

        // POST: APIContactsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Contact entity)
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
