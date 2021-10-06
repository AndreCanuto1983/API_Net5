using System;

namespace Domain.Contracts.User
{
    public class ResponseLoginOutput
    {
        public string authorization { get; set; }
        public DateTime? expires { get; set; }
        public string email { get; set; }
        public string name { get; set; }
    }
}
