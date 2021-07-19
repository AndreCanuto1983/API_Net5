using Domain.Models;
using System;
using Xunit;

namespace Test.Domain.Models
{
    public class UserModelTest
    {
        [Fact]
        public void DadoModel_Deve_RetornarObjetoPreenchido()
        {
            var user = CreateUser();

            Assert.NotNull(user);           
        }

        [Fact]
        public void DadoModel_ValidaAtributosPrincipais()
        {
            var user = CreateUser();
                        
            Assert.InRange(user.CreationDate, DateTime.MinValue, DateTime.MaxValue);
            Assert.False(user.IsDeleted);
            Assert.True(user.EmailConfirmed);
            Assert.True(user.Email.Length > 3);
            Assert.Equal("teste@gmail.com", user.Email);
            Assert.Equal("André", user.Name);
        }

        [Fact]
        public void DadoModel_ValidaSePreenchido_DadosBasicosAutomaticos()
        {
            var user = new UserModel();

            Assert.NotNull(user);
        }

        private UserModel CreateUser()
        {
            var userModel = new UserModel()
            {
                AccessFailedCount = 0,
                BirthDate = new DateTime(1983, 5, 18),
                ConcurrencyStamp = "d1a1ba91-ac65-4694-8f2d-99460",
                Cpf = "32565895231",
                CreationDate = DateTime.UtcNow,
                Email = "teste@gmail.com",
                EmailConfirmed = true,
                Id = "3sd504fg5ds0g45sdfsdf84sd5f0",
                IsDeleted = false,
                LockoutEnabled = false,
                LockoutEnd = null,
                Name = "André",
                NormalizedEmail = "TESTE@GMAIL.COM",
                NormalizedUserName = "ANDRE",
                PasswordHash = "DF5G4S65FS5D4F6SDDF688GF8J78IKJ5R",
                PhoneNumber = "16965412564",
                PhoneNumberConfirmed = true,
                SecurityStamp = "DNHQC56OSC4WYXMS2EJSJEGHMC6B",
                TwoFactorEnabled = false,
                UpdateDate = DateTime.UtcNow,
                UserLocation = "São Paulo",
                UserName = "teste@gmail.com"
            };

            return userModel;
        }
    }
}
