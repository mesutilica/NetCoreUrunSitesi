using Microsoft.AspNetCore.Mvc;

namespace WebAPIUsing.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
