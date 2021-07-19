
using Domain.Contracts.User.Input;
using Domain.Contracts.User.Output;
using Domain.Models;

namespace Domain.Contracts.User.Extensions
{
    public static class UserExtension
    {
        public static UserBasicContract UserBasic2Front(this UserModel entity)
        {
            return new UserBasicContract()
            {
                Name = entity.UserName,
                Email = entity.Email,
                Location = entity.UserLocation                
            };
        }

        public static UserContract User2Front(this UserModel entity)
        {
            return new UserContract()
            {                
                Name = entity.Name,
                Email = entity.Email,
                Location = entity.UserLocation,
                BirthDate = entity.BirthDate,
                Cpf = entity.Cpf,
                PhoneNumber = entity.PhoneNumber
            };
        }

        public static UserModel UserRegister2Back(this UserRegisterContract entity)
        {
            return new UserModel()
            {
                Name = entity.Name,
                UserName = entity.Email,
                Email = entity.Email,
                EmailConfirmed = true,
                PhoneNumber = entity.Phone,
                UserLocation = entity.UserLocation,
                BirthDate = entity.BirthDate,
                Cpf = entity.Cpf
            };
        }
    }
}
