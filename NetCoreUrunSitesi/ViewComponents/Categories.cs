using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace WebAPIUsing.ViewComponents
{
    public class Categories : ViewComponent
    {
        private readonly IService<Category> _service;

        public Categories(IService<Category> repository)
        {
            _service = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _service.GetAllAsync());
        }

    }
}
