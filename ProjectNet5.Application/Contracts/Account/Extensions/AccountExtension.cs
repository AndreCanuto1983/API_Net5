using Domain.Contracts.User;
using System;

namespace Domain.Contracts.Account.Extensions
{
    public static class AccountExtension
    {
        public static ResponseLoginOutput ConvertToResponseLoginContract(string authorization, DateTime? expires, string email, string name)
        => new()
        {
            authorization = authorization,
            email = email,
            expires = expires,
            name = name
        };
    }
}