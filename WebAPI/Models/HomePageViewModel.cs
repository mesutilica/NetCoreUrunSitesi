using Entities;
using static WebAPI.Application.UserOperations.Commands.CreateToken.CreateTokenCommand;

namespace WebAPI.Models
{
    public class HomePageViewModel
    {
        public AppUser AppUser { get; set; }
        public CreateTokenModel CreateTokenModel { get; set; }
    }
}
