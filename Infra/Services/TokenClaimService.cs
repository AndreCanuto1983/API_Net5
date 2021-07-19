using Core.Models.Base;
using Domain.Models;
using Infra.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Services
{
    public class TokenClaimService : IGenerateToken
    {
        public async Task<string> GenerateJwtClaims(string email, UserManager<UserModel> userManager, AppSettings appSettings)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var identityClaims = new ClaimsIdentity();

            var user = await userManager.FindByEmailAsync(email);

            identityClaims.AddClaims(await userManager.GetClaimsAsync(user));              

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),                    
                    new Claim(ClaimTypes.Email, user.Email.ToString())
                }),
                Issuer = appSettings.Emissor,
                Audience = appSettings.OriginValidate,
                Expires = DateTime.UtcNow.AddHours(appSettings.Expiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }       
    }    
}
