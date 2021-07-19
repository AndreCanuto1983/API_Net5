using Core.Models.Base;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Infra.Interfaces
{
    public interface IGenerateToken
    {
        Task<string> GenerateJwtClaims(string email, UserManager<UserModel> userManager, AppSettings appSettings);        
    }
}
