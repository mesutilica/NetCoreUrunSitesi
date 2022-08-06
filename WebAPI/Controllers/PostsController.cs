using Microsoft.AspNetCore.Mvc;
using Entities;
using BL;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IRepository<Post> _repository;
        private readonly IRepository<Category> _categoryRepository;

        public PostsController(IRepository<Post> repository, IRepository<Category> categoryRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
        }
        // GET: api/<PostsController>
        [HttpGet]
        public async Task<IEnumerable<Post>> GetAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET api/<PostsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> Get(int id)
        {
            var kayit = await _repository.FindAsync(id);
            if (kayit == null)
            {
                return NotFound();
            }
            return kayit;
        }

        // POST api/<PostsController>
        [HttpPost]
        public async Task<ActionResult<Post>> PostAsync(Post entity)
        {
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = entity.Id }, entity);
        }

        // PUT api/<PostsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Post>> Put(int id, Post entity)
        {
            _repository.Update(entity);

            var sonuc = await _repository.SaveChangesAsync();
            if (sonuc > 0) return NoContent();
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<PostsController>/5
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
