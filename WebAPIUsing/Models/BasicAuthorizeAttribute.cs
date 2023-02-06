using Microsoft.AspNetCore.Authorization;

namespace WebAPIUsing.Models
{
    public class BasicAuthorizeAttribute : AuthorizeAttribute
    {
        public BasicAuthorizeAttribute()
        {
            Policy = "BasicAuthentication";
        }
    }
}
