using Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IService<Product> _repository;

        public ProductsController(IService<Product> repository)
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
        public async Task<ActionResult<Product>> Get(int id)
        {
            var kayit = await _repository.FindAsync(id);
            if (kayit == null)
            {
                return NotFound();
            }
            return kayit;
        }

        // Product api/<ProductsController>
        [HttpPost]
        public async Task<ActionResult<Product>> ProductAsync(Product entity)
        {
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = entity.Id }, entity);
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Put(int id, Product entity)
        {
            _repository.Update(entity);

            var sonuc = await _repository.SaveChangesAsync();
            if (sonuc > 0) return NoContent();
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<ProductsController>/5
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
