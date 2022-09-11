using Entities;
using WebAPI.Application.UserOperations.Commands.CreateToken;

namespace WebAPI.Models
{
    public class HomePageViewModel
    {
        public AppUser AppUser { get; set; }
        public CreateTokenModel CreateTokenModel { get; set; }
    }
}
