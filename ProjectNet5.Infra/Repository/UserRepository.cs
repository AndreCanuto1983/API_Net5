using Domain.Contracts.User;
using Domain.Contracts.User.Extensions;
using Domain.Interfaces.Repository;
using Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectNet5.Applications.Contracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infra.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(
            ApplicationDbContext context,
            ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserOutput> GetUserByEmail(string email)
        {
            try
            {
                var user = await _context.User.AsNoTracking().Where(u => u.Email == email).FirstOrDefaultAsync();

                return user.ConverterToUserContract();
            }
            catch (Exception ex)
            {
                _logger.LogError("[UserRepository][GetUserByEmail] => EXCEPTION: {ex.Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<UserFullOutput>> GetUsersPagination(int skip, int take)
        {
            try
            {
                long totalRecords = await _context.User
                    .Where(u => !u.IsDeleted && u.Email != "master@gmail.com")
                    .CountAsync();

                var users = await _context.User.AsNoTracking()
                    .Where(u => !u.IsDeleted && u.Email != "master@gmail.com")
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                var totalPages = (long)Math.Ceiling(totalRecords / (double)take);

                return users.Select(u => u.ConverterToFullUserContract(totalRecords, totalPages)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("[UserRepository][GetUsersPagination] => EXCEPTION: {ex.Message}", ex.Message);
                throw;
            }
        }
    }
}
