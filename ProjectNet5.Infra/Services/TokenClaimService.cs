using Core.Models.Base;
using Domain.Interfaces.Service;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Services
{
    public class TokenClaimService : ITokenClaimService
    {
        private readonly ILogger<TokenClaimService> _logger;

        public TokenClaimService(ILogger<TokenClaimService> logger)
        {
            _logger = logger;
        }

        public async Task<string> GenerateJwtClaims(string email, UserManager<UserModel> userManager, AppSettings appSettings)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "[TokenClaimService][GenerateJwtClaims] => EXCEPTION: {ex.Message}", ex.Message);                
                throw;
            }            
        }       
    }    
}
