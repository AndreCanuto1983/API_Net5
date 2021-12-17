using Domain.Contracts.User;
using ProjectNet5.Applications.Contracts.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<UserOutput> GetUserByEmail(string email);        
        Task<IEnumerable<UserFullOutput>> GetUsersPagination(int skip, int take);        
    }
}
