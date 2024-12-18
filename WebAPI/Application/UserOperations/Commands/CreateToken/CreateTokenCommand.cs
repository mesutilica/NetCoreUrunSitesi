﻿using Core.Entities;
using Core.Models;
using Service.Abstract;
using WebAPI.TokenOperations;

namespace WebAPI.Application.UserOperations.Commands.CreateToken
{
    public class CreateTokenCommand
    {
        //private readonly IAppUserService _context;
        private readonly IService<AppUser> _repository;
        private readonly IConfiguration _configuration;
        public CreateTokenModel Model;
        public CreateTokenCommand(IConfiguration configuration, IService<AppUser> repository)//IAppUserService context, 
        {
            //_context = context;
            _configuration = configuration;
            _repository = repository;
        }
        public async Task<Token> HandleAsync()
        {
            AppUser user = await _repository.GetAsync(x => x.Email == Model.Email && x.Password == Model.Password);
            if (user is not null)
            {
                //Token üretiliyor.
                TokenHandler handler = new(_configuration);
                Token token = handler.CreateAccessToken();

                //Refresh token Users tablosuna işleniyor.
                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenExpireDate = token.Expiration.AddMinutes(30);
                _repository.Update(user);
                await _repository.SaveChangesAsync();
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
