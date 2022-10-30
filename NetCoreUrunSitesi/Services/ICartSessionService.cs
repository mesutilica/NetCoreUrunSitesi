using Entities;

namespace NetCoreUrunSitesi.Services
{
    public interface ICartSessionService
    {
        Cart GetCart();
        void SetCart(Cart cart);
    }
}
