using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using WebAPIUsing.Models;

namespace P013EStore.WebAPIUsing.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string _apiAdres = "https://localhost:7132/api/Products";
        [Route("tum-urunlerimiz")]
        public async Task<IActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres);
            return View(model);
        }
        public async Task<IActionResult> Search(string q)
        {
            // Yöntem 1 
            //var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres);
            //var model = products.Where(p => p.IsActive && p.Name.ToLower().Contains(q.ToLower()));
            // Yöntem 2
            if (q is null)
            {
                return BadRequest();
            }
            var model2 = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres + "/GetSearch/" + q);
            return View(model2);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres); // ürünleri api üzerinden çektik
            var product = products.FirstOrDefault(p => p.Id == id); // api den çektiğimiz listeden route dan gelen id ile eşleşen kaydı bulduk
            var model = new ProductDetailViewModel()
            {
                Product = product, // model içindeki ürün
                RelatedProducts = products.Where(p => p.CategoryId == product.CategoryId && p.Id != id).Take(4).ToList()// aynı kategorideki diğer ürünler
            };
            return View(model);
        }
    }
}
