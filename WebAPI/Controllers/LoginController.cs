using Core.Entities;
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
