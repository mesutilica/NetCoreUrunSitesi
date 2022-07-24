using BL;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IRepository<Post> _repository;

        public PostsController(IRepository<Post> repository)
        {
            _repository = repository;
        }

        // GET: api/<PostsController>
        [HttpGet]
        public async Task<IEnumerable<Post>> GetAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET api/<PostsController>/5
        [HttpGet("{id}")]
        public async Task<Post> GetAsync(int id)
        {
            return await _repository.FindAsync(id);
        }

        // POST api/<PostsController>
        [HttpPost]
        public async Task<ActionResult<Post>> PostAsync(Post post)
        {
            await _repository.AddAsync(post);
            await _repository.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = post.Id }, post);
        }

        // PUT api/<PostsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Post post)
        {
            if (id != post.Id) return BadRequest();
            _repository.Update(post);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/<PostsController>/5
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
