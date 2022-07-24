using BL;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IRepository<News> _repository;

        public NewsController(IRepository<News> repository)
        {
            _repository = repository;
        }

        // GET: api/<NewsController>
        [HttpGet]
        public async Task<IEnumerable<News>> GetAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET api/<NewsController>/5
        [HttpGet("{id}")]
        public async Task<News> GetAsync(int id)
        {
            return await _repository.FindAsync(id);
        }

        // POST api/<NewsController>
        [HttpPost]
        public async Task<ActionResult<News>> PostAsync(News news)
        {
            await _repository.AddAsync(news);
            await _repository.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = news.Id }, news);
        }

        // PUT api/<NewsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, News news)
        {
            if (id != news.Id) return BadRequest();
            _repository.Update(news);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/<NewsController>/5
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
