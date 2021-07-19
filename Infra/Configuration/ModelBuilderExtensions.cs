using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infra.Configuration
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasData(
                new UserModel
                {
                    UserName = "test@gmail.com",
                    Email = "test@gmail.com",
                    LockoutEnabled = true,
                    Name = "Master",
                    EmailConfirmed = true                    
                });
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {                    
                    Id = "1",
                    Name = "Master",
                    NormalizedName = "MASTER"                    
                });
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "2",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                });
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "3",
                    Name = "Commom",
                    NormalizedName = "COMMOM"
                });
        }
    }
}
