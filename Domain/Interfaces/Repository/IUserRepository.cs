using Domain.Contracts.User.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<UserContract> GetUserByEmail(string email);
        Task<IEnumerable<UserContract>> GetUsers();
    }
}
