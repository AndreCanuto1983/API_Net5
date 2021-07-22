using Domain.Contracts.User.Extensions;
using Domain.Contracts.User.Output;
using Domain.Interfaces.Repository;
using Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infra.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserContract> GetUserByEmail(string email)
        {
            var user = await _context.User.AsNoTracking().Where(u => u.Email == email).FirstOrDefaultAsync();

            return user.User2Front();
        }

        public async Task<IEnumerable<UserContract>> GetUsers()
        {
            var users = await _context.User.AsNoTracking().Where(u => !u.IsDeleted).ToListAsync();

            return users.Select(u => u.User2Front()).ToList();
        }
    }
}
