using Core.Entities;
using NetCoreUrunSitesi.ExtensionMethods;

namespace WebAPIUsing.Services
{
    public class CartSessionService
    {
        private IHttpContextAccessor _httpContextAccessor;
        public CartSessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Cart GetCart()
        {
            Cart cartToCheck = _httpContextAccessor.HttpContext.Session.GetJson<Cart>("cart");
            if (cartToCheck == null)
            {
                _httpContextAccessor.HttpContext.Session.SetJson("cart", new Cart());
                cartToCheck = _httpContextAccessor.HttpContext.Session.GetJson<Cart>("cart");
            }
            return cartToCheck;
        }

        public void SetCart(Cart cart)
        {
            _httpContextAccessor.HttpContext.Session.SetJson("cart", cart);
        }
    }
}
