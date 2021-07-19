using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Core.Services.Authorization
{
    public class ClaimsAuthorizeAttributes : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttributes(string ClaimName, string ClaimValue) : base(typeof(RequerimentClaimFilter))
        {
            Arguments = new object[] { new Claim(ClaimName, ClaimValue) };
        }
    }
}
