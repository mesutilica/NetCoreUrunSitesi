using BL;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        private readonly IRepository<AppUser> _repository;
        public AppUsersController(IRepository<AppUser> repository)
        {
            _repository = repository;
        }
        // GET: api/<AppUsersController>
        [HttpGet]
        public async Task<IEnumerable<AppUser>> GetAsync() // IEnumerable bir class ın listesi demektir
        {
            return await _repository.GetAllAsync(); // Bu metot geriye appuser listesi döndürür.
        }

        // GET api/<AppUsersController>/5
        [HttpGet("{id}")]
        public async Task<AppUser> GetAsync(int id) // Geriye 1 tane AppUser dönen metot
        {
            return await _repository.FindAsync(id); // Geriye id parametresiyle eşleşen appuser ı dön.
        }

        // POST api/<AppUsersController>
        [HttpPost]
        public async Task<ActionResult<AppUser>> PostAsync(AppUser appUser)
        {
            await _repository.AddAsync(appUser);
            await _repository.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = appUser.Id }, appUser); // Ekleme işleminden sonra geriye veritabanına eklenen kaydı döndür
        }

        // PUT api/<AppUsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, AppUser appUser)
        {
            if (id != appUser.Id) // Eğer gelen id, gelen appuser id si ile eşleşmiyorsa
            {
                return BadRequest(); // Geriye geçersiz istek hatası dön
            }
            _repository.Update(appUser);
            await _repository.SaveChangesAsync();

            return NoContent(); // Api de güncelleme sonrasında geriye nocontent dönülür
        }

        // DELETE api/<AppUsersController>/5
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
