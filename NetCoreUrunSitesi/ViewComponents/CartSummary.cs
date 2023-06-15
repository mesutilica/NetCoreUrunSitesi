using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using NetCoreUrunSitesi.ExtensionMethods;

namespace NetCoreUrunSitesi.ViewComponents
{
    public class CartSummary : ViewComponent
    {
        public string Invoke()
        {
            return HttpContext.Session.GetJson<Cart>("Cart")?.Products.Count().ToString() ?? "0";
        }
    }
}
