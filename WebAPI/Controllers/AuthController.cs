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
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginViewModel appUser)//[FromBody] 
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
            Token tokenInstance = new();
            //Oluşturulacak token ayarlarını veriyoruz.
            tokenInstance.Expiration = DateTime.Now.AddMinutes(15);

            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: _configuration["Token:Issuer"],
                audience: _configuration["Token:Audience"],
                expires: DateTime.Now.AddMinutes(10),
                notBefore: DateTime.Now,//Token üretildikten ne kadar süre sonra devreye girsin ayarlıyouz.
                signingCredentials: signingCredentials,
                claims: claims
                );
            //Token oluşturucu sınıfında bir örnek alıyoruz.
            JwtSecurityTokenHandler tokenHandler = new();

            //Token üretiyoruz.
            tokenInstance.AccessToken = tokenHandler.WriteToken(securityToken);

            //Refresh Token üretiyoruz.
            tokenInstance.RefreshToken = Guid.NewGuid().ToString();

            //Refresh token Users tablosuna işleniyor.
            account.RefreshToken = tokenInstance.RefreshToken;
            account.RefreshTokenExpireDate = tokenInstance.Expiration.AddMinutes(30);
            _service.Update(account);
            await _service.SaveChangesAsync();

            return Ok(tokenInstance);
        }
        /*
         * https://localhost:7132/api/Brands api adresi, auth ile korumaya aldık
         * postman veya swagger dan https://localhost:7132/api/Auth a user json göndererek token alıyoruz 
         * bu tokenla postmanı açıp get isteği olarak headers sekmesine sola Authorization sağa Bearer token ı yapıştırıp isteği yolla
         */
        //return Created("", new JwtTokenGenerator(_configuration).GenerateToken());
        // POST: api/AppUsers
        [HttpPost("CreateAppUser")]
        public async Task<ActionResult<AppUser>> CreateAppUser(AppUser appUser)
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
                    await _service.SaveChangesAsync();
                    return Ok(appUser);
                }
            }
            catch (Exception)
            {
                return Problem("Hata Oluştu!");
            }
        }
        [HttpGet("GetUserByUserGuid/{id}")]
        public async Task<ActionResult<AppUser>> GetUserByUserGuid(string id)
        {
            var user = await _service.FirstOrDefaultAsync(x => x.UserGuid.ToString() == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    }
}
