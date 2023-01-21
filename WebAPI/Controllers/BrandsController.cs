using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class BrandsController : ControllerBase
    {
        private readonly IService<Brand> _service;

        public BrandsController(IService<Brand> repository)
        {
            _service = repository;
        }
        // GET: api/<BrandsController>
        [HttpGet]
        public async Task<IEnumerable<Brand>> GetAsync()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<BrandsController>/5
        [HttpGet("{id}")]
        public async Task<Brand> Get(int id)
        {
            return await _service.FindAsync(id);
        }

        // POST api/<BrandsController>
        [HttpPost]
        public async Task<Brand> PostAsync(Brand brand)
        {
            await _service.AddAsync(brand);
            await _service.SaveChangesAsync();
            return brand;
        }

        // PUT api/<BrandsController>/5
        [HttpPut]
        public async Task<ActionResult<Brand>> Put(Brand brand)
        {
            _service.Update(brand);
            var sonuc = await _service.SaveChangesAsync();
            if (sonuc > 0) return NoContent();
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<BrandsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var kayit = await _service.FindAsync(id);
            if (kayit == null) 
                return BadRequest();
            _service.Delete(kayit);
            var sonuc = await _service.SaveChangesAsync();
            if (sonuc > 0) 
                return NoContent();
            return StatusCode(StatusCodes.Status304NotModified);
        }
    }
}
