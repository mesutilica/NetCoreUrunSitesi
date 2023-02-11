using Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using WebAPI.Application.UserOperations.Commands.CreateToken;
using WebAPI.Application.UserOperations.Commands.RefreshToken;
using WebAPI.TokenOperations.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IService<AppUser> _service;
        readonly IConfiguration _configuration;
        public LoginController(IService<AppUser> service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }
        // POST: api/AppUsers
        [HttpPost]
        public async Task<ActionResult<AppUser>> PostAppUser(AppUser appUser)
        {
            try
            {
                var user = await _service.FirstOrDefaultAsync(x => x.Email == appUser.Email);
                if (user != null)
                {
                    return Conflict(new { errMes = appUser.Email + " adresi sistemde zaten kayıtlı!" });
                }
                else
                {
                    appUser.CreateDate = DateTime.Now;
                    appUser.IsActive = true;
                    await _service.AddAsync(appUser);
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
            CreateTokenCommand command = new(_configuration, _service);
            command.Model = login;
            var token = command.HandleAsync();
            return token.Result;
        }

        [HttpGet("refreshToken")]
        public ActionResult<Token> RefreshToken([FromQuery] string token)//token a gelecek refreshToken değeri RefreshTokenCommand a gönderilip yenileniyor
        {
            RefreshTokenCommand command = new(_service, _configuration);
            command.RefreshToken = token;
            var resultToken = command.HandleAsync();
            return resultToken.Result;
        }
    }
}
