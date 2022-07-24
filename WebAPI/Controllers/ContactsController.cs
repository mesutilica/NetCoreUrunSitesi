using BL;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
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
        public async Task<Contact> GetAsync(int id)
        {
            return await _repository.FindAsync(id);
        }

        // POST api/<ContactsController>
        [HttpPost]
        public async Task<ActionResult<Contact>> PostAsync(Contact contact)
        {
            await _repository.AddAsync(contact);
            await _repository.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = contact.Id }, contact);
        }

        // PUT api/<ContactsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Contact contact)
        {
            if (id != contact.Id) return BadRequest();
            _repository.Update(contact);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/<ContactsController>/5
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
