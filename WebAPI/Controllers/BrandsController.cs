using BL;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
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

        [HttpGet]
        public async Task<IEnumerable<Brand>> GetAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET api/<BrandsController>/5
        [HttpGet("{id}")]
        public async Task<Brand> GetAsync(int id)
        {
            return await _repository.FindAsync(id);
        }

        // POST api/<BrandsController>
        [HttpPost]
        public async Task<ActionResult<Brand>> PostAsync(Brand brand)
        {
            brand.CreateDate = DateTime.Now;
            await _repository.AddAsync(brand);
            await _repository.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = brand.Id }, brand);
        }

        // PUT api/<BrandsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Brand brand)
        {
            if (id != brand.Id) return BadRequest();
            _repository.Update(brand);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/<BrandsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var kayit = await _repository.FindAsync(id);
            if (kayit == null)
            {
                return NotFound();
            }
            _repository.Delete(kayit);
            return NoContent();
        }
    }
}
