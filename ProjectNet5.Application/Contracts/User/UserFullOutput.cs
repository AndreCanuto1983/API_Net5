using Domain.Contracts.User;

namespace ProjectNet5.Applications.Contracts.User
{
    public class UserFullOutput
    {
        public long TotalRecords { get; set; }
        public long TotalPages { get; set; }
        public UserOutput Data { get; set; }
    }
}
