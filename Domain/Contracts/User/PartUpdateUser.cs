using System.ComponentModel.DataAnnotations;

namespace Domain.Contracts.User
{
    public class PartUpdateUser
    {
        [Required(ErrorMessage = "O Email é obrigatório")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha atual é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve possuir no mínimo 6 caracteres")]
        [MaxLength(20, ErrorMessage = "A senha deve possuir no máximo 20 caracteres")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "A nova senha é obrigatório")]        
        [MinLength(6, ErrorMessage = "A nova senha deve possuir no mínimo 6 caracteres")]
        [MaxLength(20, ErrorMessage = "A nova senha deve possuir no máximo 20 caracteres")]
        public string NewPassword { get; set; }

        public bool IsValid()
        {
            return CurrentPassword != NewPassword;
        }
    }
}
