using BL;
using Entities;
using WebAPI.TokenOperations;
using WebAPI.TokenOperations.Models;

namespace WebAPI.Application.UserOperations.Commands.CreateToken
{
    public class CreateTokenCommand
    {
        //private readonly IAppUserService _context;
        private readonly IRepository<AppUser> _repository;
        private readonly IConfiguration _configuration;
        public CreateTokenModel Model;
        public CreateTokenCommand(IConfiguration configuration, IRepository<AppUser> repository)//IAppUserService context, 
        {
            //_context = context;
            _configuration = configuration;
            _repository = repository;
        }
        public Token Handle()
        {
            AppUser user = _repository.FirstOrDefaultAsync(x => x.Email == Model.Email && x.Password == Model.Password).Result;
            if (user is not null)
            {
                //Token üretiliyor.
                TokenHandler handler = new(_configuration);
                Token token = handler.CreateAccessToken(user);

                //Refresh token Users tablosuna işleniyor.
                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenExpireDate = token.Expiration.AddMinutes(30);
                _repository.Update(user);
                //_context.Update(user);
                // _context.SaveChangesAsync();

                return token;
            }
            return null;
        }
    }
    public class CreateTokenModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
