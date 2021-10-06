using Domain.Contracts.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<UserOutput> GetUserByEmail(string email);
        Task<IEnumerable<UserOutput>> GetUsers();
    }
}
