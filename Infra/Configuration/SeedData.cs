using Core.Const;
using Domain.Models;
using Infra.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infra.Configuration
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context, UserManager<UserModel> userManager)
        {
            if (context.User.AsNoTracking().Where(u => u.Email == "master@gmail.com" && string.IsNullOrEmpty(u.PasswordHash)).Count() > 0)
            {
                var user = context.Users.AsNoTracking().Where(u => u.Email.Equals("master@gmail.com")).FirstOrDefault();
                userManager.AddPasswordAsync(user, GroupConst.master);
                context.SaveChanges();
            }

            if (!context.UserRoles.AsNoTracking().Any())
            {
                var userId = context.User.AsNoTracking().Where(u => u.Email == "master@gmail.com").Select(u => u.Id).FirstOrDefault();
                var roleId = context.Roles.AsNoTracking().Where(r => r.NormalizedName == "MASTER").Select(u => u.Id).FirstOrDefault();

                if(!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(roleId))
                {
                    context.UserRoles.Add(entity: new IdentityUserRole<string> { UserId = userId, RoleId = roleId });
                    context.SaveChanges();
                }                
            }

            userManager.Dispose();
        }
    }
}
