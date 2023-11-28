using Core.Entities;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.Abstract;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IService<AppUser> _service;
        readonly IConfiguration _configuration;
        public AuthController(IService<AppUser> service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(AdminLoginViewModel appUser)//[FromBody] 
        {
            var account = await _service.FirstOrDefaultAsync(u => u.Email == appUser.Email && u.Password == appUser.Password && u.IsActive);
            if (account == null)
            {
                return NotFound();
            }
            //Security  Key'in simetriğini alıyoruz.
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            //Şifrelenmiş kimliği oluşturuyoruz.
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            // Eğer rol bazlı yapacaksak
            var claims = new List<Claim>() // Claim = hak
                        {
                            new Claim(ClaimTypes.Name, account.Username),
                            new Claim(ClaimTypes.Role, account.IsAdmin ? "Admin" : "User"),
                            new Claim("UserId", account.Id.ToString())
                        };

            //Oluşturulacak token ayarlarını veriyoruz.
            //tokenInstance.Expiration = DateTime.Now.AddMinutes(15);
            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: _configuration["Token:Issuer"],
                audience: _configuration["Token:Audience"],
                expires: DateTime.Now.AddMinutes(1),
                notBefore: DateTime.Now,//Token üretildikten ne kadar süre sonra devreye girsin ayarlıyouz.
                signingCredentials: signingCredentials,
                claims: claims
                );
            //Token oluşturucu sınıfında bir örnek alıyoruz.
            JwtSecurityTokenHandler tokenHandler = new();
            return Created("", tokenHandler.WriteToken(securityToken));
        }
        /*
         * https://localhost:7132/api/Brands api adresi, auth ile korumaya aldık
         * postman veya swagger dan https://localhost:7132/api/Auth a user json göndererek token alıyoruz 
         * bu tokenla postmanı açıp get isteği olarak headers sekmesine sola Authorization sağa Bearer token ı yapıştırıp isteği yolla
         */
        //return Created("", new JwtTokenGenerator(_configuration).GenerateToken());
    }
}
