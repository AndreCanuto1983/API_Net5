using Domain.Models;
using ProjectNet5.Applications.Contracts.User;

namespace Domain.Contracts.User.Extensions
{
    public static class UserExtension
    {
        public static UserBasicOutput ConverterToUserBasicContract(this UserModel entity)
        => new()
        {
            Name = entity.UserName,
            Email = entity.Email,
            Location = entity.UserLocation
        };

        public static UserOutput ConverterToUserContract(this UserModel entity)
        => new()
        {
            Name = entity.Name,
            Email = entity.Email,
            Location = entity.UserLocation,
            BirthDate = entity.BirthDate,
            Cpf = entity.Cpf,
            PhoneNumber = entity.PhoneNumber
        };

        public static UserFullOutput ConverterToFullUserContract(this UserModel entity, long totalRecords, long totalPages)
        => new()
        {
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            Data = new UserOutput()
            {
                Name = entity.Name,
                Email = entity.Email,
                Location = entity.UserLocation,
                BirthDate = entity.BirthDate,
                Cpf = entity.Cpf,
                PhoneNumber = entity.PhoneNumber
            }
        };

        public static UserModel ConverterToUserModel(this UserRegisterInput entity)
        => new()
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

        public static UserModel ConverterToUserModelForUpdate(ref UserModel userModel, ref UserUpdateInput entity)
        {
            userModel.Name = entity.Name;
            userModel.UserName = entity.Email;
            userModel.Email = entity.Email;
            userModel.EmailConfirmed = true;
            userModel.PhoneNumber = entity.Phone;
            userModel.UserLocation = entity.UserLocation;
            userModel.BirthDate = entity.BirthDate;
            userModel.Cpf = entity.Cpf;

            return userModel;
        }
    }
}
