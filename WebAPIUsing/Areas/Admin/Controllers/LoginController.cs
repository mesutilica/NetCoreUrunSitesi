using Microsoft.AspNetCore.Mvc;

namespace WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
