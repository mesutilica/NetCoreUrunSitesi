using NetCoreUrunSitesi.ExtensionMethods;
using Service.Concrete;

namespace WebAPIUsing.Services
{
    public class CartSessionService : ICartSessionService
    {
        private IHttpContextAccessor _httpContextAccessor;
        public CartSessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public CartService GetCart()
        {
            CartService cartToCheck = _httpContextAccessor.HttpContext.Session.GetJson<CartService>("cart");
            if (cartToCheck == null)
            {
                _httpContextAccessor.HttpContext.Session.SetJson("cart", new CartService());
                cartToCheck = _httpContextAccessor.HttpContext.Session.GetJson<CartService>("cart");
            }
            return cartToCheck;
        }

        public void SetCart(CartService cart)
        {
            _httpContextAccessor.HttpContext.Session.SetJson("cart", cart);
        }
    }
}
