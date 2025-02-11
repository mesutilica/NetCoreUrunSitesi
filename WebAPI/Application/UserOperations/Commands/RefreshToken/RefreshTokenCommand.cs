﻿using Core.Entities;
using Core.Models;
using Service.Abstract;
using WebAPI.TokenOperations;

namespace WebAPI.Application.UserOperations.Commands.RefreshToken
{
    public class RefreshTokenCommand
    {
        //private readonly IAppUserService _context;
        private readonly IService<AppUser> _repository;
        private readonly IConfiguration _configuration;
        public string RefreshToken;
        public RefreshTokenCommand(IService<AppUser> context, IConfiguration configuration)
        {
            _repository = context;
            _configuration = configuration;
        }
        public async Task<Token> HandleAsync()
        {
            AppUser user = _repository.GetAsync(x => x.RefreshToken == RefreshToken && x.RefreshTokenExpireDate > DateTime.Now).Result;
            if (user is not null)
            {

                TokenHandler tokenHandler = new(_configuration);
                Token token = tokenHandler.CreateAccessToken();

                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenExpireDate = token.Expiration.AddMinutes(60);
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
