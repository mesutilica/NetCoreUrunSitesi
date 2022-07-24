using BL;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreUrunSitesi.ViewComponents
{
    public class Categories : ViewComponent
    {
        private readonly IRepository<Category> _repository;

        public Categories(IRepository<Category> repository)
        {
            _repository = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _repository.GetAllAsync());
        }

    }
}
