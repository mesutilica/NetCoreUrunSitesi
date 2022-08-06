using Microsoft.AspNetCore.Mvc;
using Entities;
using BL;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IRepository<Contact> _repository;

        public ContactsController(IRepository<Contact> repository)
        {
            _repository = repository;
        }
        // GET: api/<ContactsController>
        [HttpGet]
        public async Task<IEnumerable<Contact>> GetAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET api/<ContactsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> Get(int id)
        {
            var kayit = await _repository.FindAsync(id);
            if (kayit == null)
            {
                return NotFound();
            }
            return kayit;
        }

        // POST api/<ContactsController>
        [HttpPost]
        public async Task<ActionResult<Contact>> PostAsync(Contact entity)
        {
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = entity.Id }, entity);
        }

        // PUT api/<ContactsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Contact>> Put(int id, Contact entity)
        {
            _repository.Update(entity);

            var sonuc = await _repository.SaveChangesAsync();
            if (sonuc > 0) return NoContent();
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<ContactsController>/5
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
