using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace WebAPIUsing.Controllers
{
    public class NewsController : Controller
    {
        private readonly IService<News> _newsRepository;

        public NewsController(IService<News> newsRepository)
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
