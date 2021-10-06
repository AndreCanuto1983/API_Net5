using System.ComponentModel.DataAnnotations;

namespace Domain.Contracts.User
{
    public class UserForgotPasswordInput
    {
        [Required(ErrorMessage = "O campo Email é obrigatório")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
