using Service.Concrete;

namespace WebAPIUsing.Services
{
    public interface ICartSessionService
    {
        CartService GetCart();
        void SetCart(CartService cart);
    }
}
