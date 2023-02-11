using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace WebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        private readonly IService<AppUser> _service;

        public AppUsersController(IService<AppUser> service)
        {
            _service = service;
        }

        // GET: api/<AppUsersController>
        [HttpGet]
        public async Task<IEnumerable<AppUser>> GetAsync()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<AppUsersController>/5
        [HttpGet("{id}")]
        public async Task<AppUser> Get(int id)
        {
            return await _service.FindAsync(id);
        }

        // POST api/<AppUsersController>
        [HttpPost]
        public async Task<AppUser> PostAsync(AppUser appUser)
        {
            await _service.AddAsync(appUser);
            await _service.SaveChangesAsync();

            return appUser; // ekleme işleminden sonra geriye eklenen kaydı döndür
            // Postman ile kayıt eklemek için uygulamada yeni sekme açıp metot türünü post seçiyoruz.
            // post yapılacak url adres kısmına swagger dan aldığımız uygulama adresini(https://localhost:7132/api/AppUsers) request url kısmından alabiliriz postmandaki adres kısmına yapıştırıyoruz.
            // Adres kısmının altından Body seçeneğini seçip oradan raw ı ve veri tipi olarak en sağdaki listeden Json ı seçiyoruz.
            // Json türünde ilgili eklenecek verimizi gönderiyoruz.(örnek json swagger uı dan alınabilir)
            // Send butonuna basıp isteği yolluyoruz. 201 dönerse işlem başırılıdır
        }

        // PUT api/<AppUsersController>/5
        [HttpPut]
        public async Task<ActionResult<AppUser>> Put(AppUser appUser) // Güncelleme için Put metodu kullanılır
        {
            _service.Update(appUser);
            var sonuc = await _service.SaveChangesAsync();
            if (sonuc > 0) return NoContent(); // güncellemenin geri dönüş türü no content
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<AppUsersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var appUser = await _service.FindAsync(id);
            if (appUser == null) return BadRequest();
            _service.Delete(appUser);

            var sonuc = await _service.SaveChangesAsync();
            if (sonuc > 0) return Ok();
            return StatusCode(StatusCodes.Status304NotModified);
        }
    }
}
