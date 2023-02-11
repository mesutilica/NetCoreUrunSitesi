using Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using WebApi.Utils;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {
        private readonly IService<Slider> _service;

        public SlidersController(IService<Slider> repository)
        {
            _service = repository;
        }
        // GET: api/<SliderController>
        [HttpGet]
        public async Task<IEnumerable<Slider>> GetAsync()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<SliderController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Slider>> Get(int id)
        {
            var kayit = await _service.FindAsync(id);
            if (kayit == null)
                return NotFound();
            return kayit;
        }

        // POST api/<SliderController>
        [HttpPost]
        public async Task<Slider> PostAsync(Slider entity, [FromForm] IFormFile? formFile)
        {
            var result = await FileHelper.FileLoaderAsync(formFile);
            await _service.AddAsync(entity);
            await _service.SaveChangesAsync();
            return entity;
        }

        // PUT api/<SliderController>/5
        [HttpPut]
        public async Task<ActionResult<Slider>> Put(Slider entity)
        {
            _service.Update(entity);
            var sonuc = await _service.SaveChangesAsync();
            if (sonuc > 0) return NoContent();
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<SliderController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var kayit = await _service.FindAsync(id);
            if (kayit == null) 
                return BadRequest();
            _service.Delete(kayit);
            var sonuc = await _service.SaveChangesAsync();
            if (sonuc > 0) 
                return Ok();
            return StatusCode(StatusCodes.Status304NotModified);
        }
    }
}
