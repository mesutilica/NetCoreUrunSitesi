﻿using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IService<News> _repository;

        public NewsController(IService<News> repository)
        {
            _repository = repository;
        }
        // GET: api/<NewsController>
        [HttpGet]
        public async Task<IEnumerable<News>> GetAsync()
        {
            return await _repository.GetAllAsync();
        }

        // GET api/<NewsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> Get(int id)
        {
            var kayit = await _repository.FindAsync(id);
            if (kayit == null)
            {
                return NotFound();
            }
            return kayit;
        }

        // POST api/<NewsController>
        [HttpPost]
        public async Task<News> PostAsync(News entity)
        {
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return entity;
        }

        // PUT api/<NewsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<News>> Put(News entity)
        {
            _repository.Update(entity);
            var sonuc = await _repository.SaveChangesAsync();
            if (sonuc > 0) return NoContent();
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<NewsController>/5
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
