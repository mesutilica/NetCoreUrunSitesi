using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.ExtensionMethods;
using Service.Abstract;

namespace NetCoreUrunSitesi.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IProductService _productService;

        public FavoritesController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            var favoriler = GetFavorites();
            return View(favoriler);
        }
        private List<Product> GetFavorites()
        {
            return HttpContext.Session.GetJson<List<Product>>("GetFavorites") ?? [];
        }
        public IActionResult Add(int ProductId)
        {
            var product = _productService.Find(ProductId);
            var favoriler = GetFavorites();

            if (product != null && !favoriler.Any(p => p.Id == ProductId))
            {
                favoriler.Add(product);
                HttpContext.Session.SetJson("GetFavorites", favoriler);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int ProductId)
        {
            var product = _productService.Find(ProductId);
            var favoriler = GetFavorites();

            if (product != null && favoriler.Any(p => p.Id == ProductId))
            {
                favoriler.RemoveAll(i => i.Id == product.Id);
                HttpContext.Session.SetJson("GetFavorites", favoriler);
            }
            return RedirectToAction("Index");
        }
    }
}
