using Microsoft.AspNetCore.Mvc;
using Entities;
using BL;

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
        public async Task<IEnumerable<AppUser>> GetAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET api/<AppUsersController>/5
        [HttpGet("{id}")]
        public async Task<AppUser> Get(int id)
        {
            return await _repository.FindAsync(id);
        }

        // POST api/<AppUsersController>
        [HttpPost]
        public async Task<ActionResult<AppUser>> PostAsync(AppUser appUser)
        {
            await _repository.AddAsync(appUser);
            await _repository.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = appUser.Id }, appUser); // ekleme işleminden sonra geriye eklenen kaydı döndür
            // Postman ile kayıt eklemek için uygulamada yeni sekme açıp metot türünü post seçiyoruz.
            // post yapılacak url adres kısmına swagger dan aldığımız uygulama adresini(https://localhost:7132/api/AppUsers) request url kısmından alabiliriz postmandaki adres kısmına yapıştırıyoruz.
            // Adres kısmının altından Body seçeneğini seçip oradan raw ı ve veri tipi olarak en sağdaki listeden Json ı seçiyoruz.
            // Json türünde ilgili eklenecek verimizi gönderiyoruz.(örnek json swagger uı dan alınabilir)
            // Send butonuna basıp isteği yolluyoruz. 201 dönerse işlem başırılıdır
        }

        // PUT api/<AppUsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AppUser>> Put(int id, AppUser appUser) // Güncelleme için Put metodu kullanılır
        {
            _repository.Update(appUser);
            var sonuc = await _repository.SaveChangesAsync();
            if (sonuc > 0) return NoContent(); // güncellemenin geri dönüş türü no content
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<AppUsersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var appUser = await _repository.FindAsync(id);
            if (appUser == null) return BadRequest();
            _repository.Delete(appUser);

            var sonuc = await _repository.SaveChangesAsync();
            if (sonuc > 0) return Ok();
            return StatusCode(StatusCodes.Status304NotModified);
        }
    }
}
