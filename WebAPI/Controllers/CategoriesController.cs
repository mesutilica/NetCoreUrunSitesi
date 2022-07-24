using BL;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepository<Category> _repository;

        public CategoriesController(IRepository<Category> repository)
        {
            _repository = repository;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<IEnumerable<Category>> GetAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<Category> GetAsync(int id)
        {
            return await _repository.FindAsync(id);
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<ActionResult<Category>> PostAsync(Category category)
        {
            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = category.Id }, category);
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Category category)
        {
            if (id != category.Id) return BadRequest();
            _repository.Update(category);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var kayit = await _repository.FindAsync(id);
            if (kayit == null) return NotFound();
            _repository.Delete(kayit);
            return NoContent();
        }
    }
}
