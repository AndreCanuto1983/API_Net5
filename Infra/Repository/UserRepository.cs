using Domain.Contracts.User.Extensions;
using Domain.Contracts.User;
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

        public async Task<UserOutput> GetUserByEmail(string email)
        {
            var user = await _context.User.AsNoTracking().Where(u => u.Email == email).FirstOrDefaultAsync();

            return user.ConverterToUserContract();
        }

        public async Task<IEnumerable<UserOutput>> GetUsers()
        {
            var users = await _context.User.AsNoTracking().Where(u => !u.IsDeleted && u.Email != "master@gmail.com").ToListAsync();

            return users.Select(u => u.ConverterToUserContract()).ToList();
        }
    }
}
