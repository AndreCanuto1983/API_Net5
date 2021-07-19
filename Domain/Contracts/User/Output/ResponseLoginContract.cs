using System;

namespace Domain.Contracts.User.Output
{
    public class ResponseLoginContract
    {
        public string authorization { get; set; }
        public DateTime? expires { get; set; }
        public string email { get; set; }
        public string name { get; set; }
    }
}
