using Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.ExtensionMethods;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Service.Abstract;
using Service.Concrete;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace NetCoreUrunSitesi.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IProductService _productService;
        private readonly IService<AppUser> _userService;
        private readonly INotyfService _notifyService;

        public FavoritesController(IProductService productService, IService<AppUser> userService, INotyfService notifyService)
        {
            _productService = productService;
            _userService = userService;
            _notifyService = notifyService;
        }
        public async Task<IActionResult> Index()
        {
            var favoriler = await GetFavoritesAsync();
            return View(favoriler);
        }
        private async Task<List<Product>> GetFavoritesAsync()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var appUser = await _userService.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
                if (appUser == null)
                {
                    await HttpContext.SignOutAsync();
                    //return HttpContext.Session.GetJson<List<Product>>("GetFavorites") ?? [];
                }
                if (!string.IsNullOrWhiteSpace(appUser.RefreshToken))
                    HttpContext.Session.SetJson("GetFavorites", JsonConvert.DeserializeObject<List<Product>>(appUser.RefreshToken));
            }
            return HttpContext.Session.GetJson<List<Product>>("GetFavorites") ?? [];
        }
        public async Task<IActionResult> AddAsync(int ProductId)
        {
            var product = _productService.Find(ProductId);
            var favoriler = await GetFavoritesAsync();

            if (product != null && !favoriler.Any(p => p.Id == ProductId))
            {
                favoriler.Add(product);
                HttpContext.Session.SetJson("GetFavorites", favoriler);
                _notifyService.Success(product.Name + " Favorilere Eklendi");

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var appUser = await _userService.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
                    appUser.RefreshToken = JsonConvert.SerializeObject(favoriler).ToString();
                    await _userService.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveAsync(int ProductId)
        {
            var product = _productService.Find(ProductId);
            var favoriler = await GetFavoritesAsync();

            if (product != null && favoriler.Any(p => p.Id == ProductId))
            {
                favoriler.RemoveAll(i => i.Id == product.Id);
                HttpContext.Session.SetJson("GetFavorites", favoriler);
                _notifyService.Error(product.Name + " Favorilerden kaldırıldı..");

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var appUser = await _userService.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
                    appUser.RefreshToken = JsonConvert.SerializeObject(favoriler).ToString();
                    await _userService.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
