using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Core.Services.Authorization
{
    public class CustomAuthorization
    {
        public static bool ValidateUserClaim(HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity.IsAuthenticated &&
                context.User.Claims.Any(c => c.Type == claimName && c.Value.Split(',').Contains(claimValue));
        }
    }    
}
