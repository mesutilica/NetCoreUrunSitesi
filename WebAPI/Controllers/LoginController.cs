using BL;
using Entities;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.UserOperations.Commands.CreateToken;
using WebAPI.Application.UserOperations.Commands.RefreshToken;
using WebAPI.TokenOperations.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IRepository<AppUser> _repository;
        readonly IConfiguration _configuration;
        public LoginController(IRepository<AppUser> repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }
        // POST: api/AppUsers
        [HttpPost]
        public async Task<ActionResult<AppUser>> PostAppUser(AppUser appUser)
        {
            try
            {
                var user = await _repository.FirstOrDefaultAsync(x => x.Email == appUser.Email);
                if (user != null)
                {
                    return Conflict(new { errMes = appUser.Email + " adresi sistemde zaten kayıtlı!" });
                }
                else
                {
                    appUser.CreateDate = DateTime.Now;
                    appUser.IsActive = true;
                    await _repository.AddAsync(appUser);
                    return CreatedAtAction("GetAppUser", new { id = appUser.Id }, appUser);
                }
            }
            catch (Exception)
            {
                return Problem("Hata Oluştu!");
            }
        }

        [HttpPost("connect/token")]
        public ActionResult<Token> Login(CreateTokenModel login)//[FromBody] 
        {
            CreateTokenCommand command = new CreateTokenCommand(_configuration, _repository);
            command.Model = login;
            var token = command.Handle();
            return token;
        }

        [HttpGet("refreshToken")]
        public ActionResult<Token> RefreshToken([FromQuery] string token)//token a gelecek refreshToken değeri RefreshTokenCommand a gönderilip yenileniyor
        {
            RefreshTokenCommand command = new RefreshTokenCommand(_repository, _configuration);
            command.RefreshToken = token;
            var resultToken = command.Handle();
            return resultToken;
        }
    }
}
