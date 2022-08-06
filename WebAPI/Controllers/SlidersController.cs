using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BL;
using Entities;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {
        private readonly IRepository<Slider> _repository;

        public SlidersController(IRepository<Slider> repository)
        {
            _repository = repository;
        }
        // GET: api/<SliderController>
        [HttpGet]
        public async Task<IEnumerable<Slider>> GetAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET api/<SliderController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Slider>> Get(int id)
        {
            var kayit = await _repository.FindAsync(id);
            if (kayit == null)
            {
                return NotFound();
            }
            return kayit;
        }

        // POST api/<SliderController>
        [HttpPost]
        public async Task<ActionResult<Slider>> PostAsync(Slider entity)
        {
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = entity.Id }, entity);
        }

        // PUT api/<SliderController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Slider>> Put(int id, Slider entity)
        {
            _repository.Update(entity);

            var sonuc = await _repository.SaveChangesAsync();
            if (sonuc > 0) return NoContent();
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<SliderController>/5
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
