using Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Abstract;

namespace NetCoreUrunSitesi.Controllers
{
    public class ProductsController : Controller
    {
        /*private readonly IRepository<Product> _productRepository;

        public ProductsController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }*/
        private readonly IProductService _productRepository;
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(IProductService productRepository, HttpClient httpClient, IHttpClientFactory httpClientFactory)
        {
            _productRepository = productRepository;
            _httpClient = httpClient;
            _apiAdres = "https://localhost:7132/Api";
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(int id)
        {
            return View(await _productRepository.GetAllAsync(p => p.CategoryId == id));
        }

        public async Task<IActionResult> Search(string q)
        {
            return View(await _productRepository.GetAllAsync(p => p.Name.Contains(q)));
        }

        public async Task<IActionResult> Detail(int id)
        {
            //var model = await _productRepository.GetProductByCategoryAndBrandAsync(id);
            /* çalışan kod
            var response = await _httpClient.GetAsync($"{_apiAdres}/Products/{id}");
            if (response.IsSuccessStatusCode)
            {
                var respBody = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(respBody);
                return View(product);
            }*/
            /* çalışan 2
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"{_apiAdres}/Products/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Product>(jsonData);
                return View(data);
            }
            */
            var data = await _httpClient.GetFromJsonAsync<Product>($"{_apiAdres}/Products/{id}");
            return View(data);
        }
    }
}
