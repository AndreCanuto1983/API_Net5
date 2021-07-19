using Domain.Contracts.User.Output;
using System;

namespace Domain.Contracts.Account.Extensions
{
    public static class AccountExtension
    {
        public static ResponseLoginContract ResponseLogin2Front(string authorization, DateTime? expires, string email, string name)
        {
            return new ResponseLoginContract()
            {
                authorization = authorization,
                email = email,
                expires = expires,
                name = name
            };
        }
    }
}