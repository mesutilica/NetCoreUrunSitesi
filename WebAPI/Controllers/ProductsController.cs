using BL;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _repository;

        public ProductsController(IRepository<Product> repository)
        {
            _repository = repository;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IEnumerable<Product>> GetAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<Product> GetAsync(int id)
        {
            return await _repository.FindAsync(id);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<ActionResult<Product>> PostAsync(Product product)
        {
            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = product.Id }, product);
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Product product)
        {
            if (id != product.Id) return BadRequest();
            _repository.Update(product);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var kayit = await _repository.FindAsync(id);
            if (kayit == null) return NotFound();
            _repository.Delete(kayit);
            return NoContent();
        }
    }
}
