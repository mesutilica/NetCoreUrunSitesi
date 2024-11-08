using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.ExtensionMethods;
using Service.Concrete;

namespace NetCoreUrunSitesi.ViewComponents
{
    public class CartSummary : ViewComponent
    {
        public string Invoke()
        {
            return HttpContext.Session.GetJson<CartService>("Cart")?.CartLines.Count.ToString() ?? "0";
        }
    }
}
