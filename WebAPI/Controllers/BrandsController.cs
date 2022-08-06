using Microsoft.AspNetCore.Mvc;
using Entities;
using BL;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IRepository<Brand> _repository;

        public BrandsController(IRepository<Brand> repository)
        {
            _repository = repository;
        }
        // GET: api/<BrandsController>
        [HttpGet]
        public async Task<IEnumerable<Brand>> GetAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET api/<BrandsController>/5
        [HttpGet("{id}")]
        public async Task<Brand> Get(int id)
        {
            return await _repository.FindAsync(id);
        }

        // POST api/<BrandsController>
        [HttpPost]
        public async Task<ActionResult<Brand>> PostAsync(Brand brand)
        {
            await _repository.AddAsync(brand);
            await _repository.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = brand.Id }, brand);
        }

        // PUT api/<BrandsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Brand>> Put(int id, Brand brand)
        {
            _repository.Update(brand);
            var sonuc = await _repository.SaveChangesAsync();
            if (sonuc > 0) return NoContent();
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<BrandsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var kayit = await _repository.FindAsync(id);
            if (kayit == null) return BadRequest();
            _repository.Delete(kayit);

            var sonuc = await _repository.SaveChangesAsync();
            if (sonuc > 0) return NoContent();
            return StatusCode(StatusCodes.Status304NotModified);
        }
    }
}
