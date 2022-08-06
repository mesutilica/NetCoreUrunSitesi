using Microsoft.AspNetCore.Mvc;
using Entities;
using BL;

namespace WebAPI.Controllers
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
        public async Task<Category> Get(int id)
        {
            return await _repository.FindAsync(id);
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<ActionResult<Category>> PostAsync(Category entity)
        {
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = entity.Id }, entity);
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> Put(int id, Category entity)
        {
            _repository.Update(entity);

            var sonuc = await _repository.SaveChangesAsync();
            if (sonuc > 0) return NoContent();
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var kayit = await _repository.FindAsync(id);
            if (kayit == null) return BadRequest();
            _repository.Delete(kayit);

            var sonuc = await _repository.SaveChangesAsync();
            if (sonuc > 0) return Ok();
            return StatusCode(StatusCodes.Status304NotModified);
        }
    }
}
