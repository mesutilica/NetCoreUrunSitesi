using BL;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebApi.Utils;

namespace WebApi.Controllers
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

        // GET: api/<SlidersController>
        [HttpGet]
        public async Task<IEnumerable<Slider>> GetAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET api/<SlidersController>/5
        [HttpGet("{id}")]
        public async Task<Slider> GetAsync(int id)
        {
            return await _repository.FindAsync(id);
        }

        // POST api/<SlidersController>
        [HttpPost]
        public async Task<ActionResult<Slider>> PostAsync(Slider slider)//, IFormFile Image
        {
            //slider.Image = await FileHelper.FileLoader(Image);
            await _repository.AddAsync(slider);
            await _repository.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = slider.Id }, slider);
        }

        // PUT api/<SlidersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Slider slider)
        {
            if (id != slider.Id) return BadRequest();
            _repository.Update(slider);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/<SlidersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var kayit = await _repository.FindAsync(id);
            if (kayit == null) return NotFound();
            _repository.Delete(kayit);
            return NoContent();
        }

        // POST api/<SlidersController>
        [HttpPost("Upload")] // Slider controller da resim yükleme actionu
        public async Task<IActionResult> Upload(IFormFile formFile) // Metot ismi Upload, parametre olarak Iformfile ile bir formdan gelecek dosyayı alıyor
        {
            // Backend olarak api kullanacaksak resim yüklemek için projeye wwwroot klasörü oluşturup startup.cs dosyasına app.usestaticfile özelliğini eklememiz gerekir!!

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", formFile.FileName); // burada dosyayı yükleyeceğimiz path yani yolu ayarladık
            var stream = new FileStream(path, FileMode.Create); // dosya yükleme için stream yani akış oluşturduk ve mod olarak yeni ekleme için filemode create ayarladık
            await formFile.CopyToAsync(stream); // Dosyayı asenkron olarak sunucuya kopyaladık
            return Created(string.Empty, formFile); // Geriye dosyanın eklendiğine dair response döndük
        }

    }
}
