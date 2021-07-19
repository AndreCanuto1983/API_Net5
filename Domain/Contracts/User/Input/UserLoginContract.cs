using System.ComponentModel.DataAnnotations;

namespace Domain.Contracts.User.Input
{
    public class UserLoginContract
    {
        [Required(ErrorMessage = "O campo Email é obrigatório")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Password é obrigatório")]
        [MinLength(6, ErrorMessage = "O campo Password deve possuir no mínimo 6 caracteres")]
        [MaxLength(20, ErrorMessage = "O campo Password deve possuir no máximo 20 caracteres")]
        public string Password { get; set; }
    }
}
