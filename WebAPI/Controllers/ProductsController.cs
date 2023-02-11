using Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IService<Product> _service;
        private readonly IProductService _productService;

        public ProductsController(IService<Product> repository, IProductService productService)
        {
            _service = repository;
            _productService = productService;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IEnumerable<Product>> GetAsync()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var kayit = await _productService.GetProductByCategoryAndBrandAsync(id);//_repository.FindAsync(id);
            if (kayit == null)
                return NotFound();
            return kayit;
        }

        // Product api/<ProductsController>
        [HttpPost]
        public async Task<ActionResult<Product>> PostAsync(Product entity)
        {
            await _service.AddAsync(entity);
            await _service.SaveChangesAsync();
            return entity;
        }

        // PUT api/<ProductsController>/5
        [HttpPut]
        public async Task<ActionResult<Product>> Put(int id, Product entity)
        {
            _service.Update(entity);
            var sonuc = await _service.SaveChangesAsync();
            if (sonuc > 0) return NoContent();
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var kayit = await _service.FindAsync(id);
            if (kayit == null) return BadRequest();
            _service.Delete(kayit);

            var sonuc = await _service.SaveChangesAsync();
            if (sonuc > 0) return Ok();
            return StatusCode(StatusCodes.Status304NotModified);
        }
    }
}
