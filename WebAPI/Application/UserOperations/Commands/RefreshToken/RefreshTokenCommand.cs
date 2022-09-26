using BL;
using Entities;
using WebAPI.TokenOperations;
using WebAPI.TokenOperations.Models;

namespace WebAPI.Application.UserOperations.Commands.RefreshToken
{
    public class RefreshTokenCommand
    {
        //private readonly IAppUserService _context;
        private readonly IRepository<AppUser> _repository;
        private readonly IConfiguration _configuration;
        public string RefreshToken;
        public RefreshTokenCommand(IRepository<AppUser> context, IConfiguration configuration)
        {
            _repository = context;

            _configuration = configuration;
        }
        public async Task<Token> HandleAsync()
        {
            AppUser user = _repository.FirstOrDefaultAsync(x => x.RefreshToken == RefreshToken && x.RefreshTokenExpireDate > DateTime.Now).Result;
            if (user is not null)
            {

                TokenHandler tokenHandler = new(_configuration);
                Token token = tokenHandler.CreateAccessToken(user);

                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenExpireDate = token.Expiration.AddMinutes(30);
                _repository.Update(user);
                await _repository.SaveChangesAsync();

                return token;
            }
            else
            {
                throw new InvalidOperationException("Valid Refresh Token Bulunamadı");
            }
        }
    }
}
