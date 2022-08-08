using Microsoft.AspNetCore.Mvc;
using BL;
using Entities;

namespace NetCoreUrunSitesi.Controllers
{
    public class NewsController : Controller
    {
        private readonly IRepository<News> _newsRepository;

        public NewsController(IRepository<News> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IActionResult> IndexAsync()
        {
            return View(await _newsRepository.GetAllAsync());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            return View(await _newsRepository.FindAsync(id.Value));
        }
    }
}
