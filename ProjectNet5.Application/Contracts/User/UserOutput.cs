using System;

namespace Domain.Contracts.User
{
    public class UserOutput
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string Cpf { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
