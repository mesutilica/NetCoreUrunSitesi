using Microsoft.AspNetCore.Authorization;

namespace NetCoreUrunSitesi.Models
{
    public class BasicAuthorizeAttribute : AuthorizeAttribute
    {
        public BasicAuthorizeAttribute()
        {
            Policy = "BasicAuthentication";
        }
    }
}
