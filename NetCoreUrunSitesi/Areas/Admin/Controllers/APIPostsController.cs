using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreUrunSitesi.Areas.Admin.Controllers
{
    public class APIPostsController : Controller
    {
        // GET: APIPostsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: APIPostsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: APIPostsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: APIPostsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: APIPostsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: APIPostsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: APIPostsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: APIPostsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
