using System.ComponentModel.DataAnnotations;

namespace Domain.Contracts.User.Input
{
    public class UserForgotPasswordContract
    {
        [Required(ErrorMessage = "O campo Email é obrigatório")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
