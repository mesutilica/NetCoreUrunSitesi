using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.UserId = HttpContext.User.FindFirst("UserId").Value;
            return View();
        }
    }
}
