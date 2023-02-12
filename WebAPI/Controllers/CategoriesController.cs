using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        //private readonly IRepository<Category> _repository;
        private readonly IService<Category> _service;
        private readonly ICategoryService _categoryService;
        public CategoriesController(IService<Category> service, ICategoryService categoryService)
        {
            _service = service;
            _categoryService = categoryService;
        }
        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<IEnumerable<Category>> GetAsync()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<Category> Get(int id)
        {
            return await _service.FindAsync(id);
        }

        // GET api/<CategoriesController>/5
        [HttpGet("GetCategoryByProducts/{categoryId}")]
        public async Task<Category> GetCategoryByProducts(int categoryId)
        {
            return await _categoryService.GetCategoryByProductsAsync(categoryId);
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<Category> PostAsync(Category entity)
        {
            await _service.AddAsync(entity);
            await _service.SaveChangesAsync();
            return entity;
        }

        // PUT api/<CategoriesController>/5
        [HttpPut]
        public async Task<ActionResult<Category>> Put(Category entity)
        {
            _service.Update(entity);
            var sonuc = await _service.SaveChangesAsync();
            if (sonuc > 0) return NoContent();
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var kayit = await _service.FindAsync(id);
            if (kayit == null) return BadRequest();
            _service.Delete(kayit);
            var sonuc = await _service.SaveChangesAsync();
            if (sonuc > 0) return Ok();
            return StatusCode(StatusCodes.Status304NotModified);
        }
    }
}
