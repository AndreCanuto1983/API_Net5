using System.ComponentModel.DataAnnotations;

namespace Domain.Contracts.User
{
    public class UserResetPasswordInput
    {
        [Required(ErrorMessage = "O Email é obrigatório")]        
        [EmailAddress]
        public string Email { get; set; }
                
        [Required(ErrorMessage = "O Password é obrigatório")]        
        [MinLength(4, ErrorMessage = "O campo Password deve possuir no mínimo 4 caracteres")]
        [MaxLength(20, ErrorMessage = "O campo Password deve possuir no máximo 20 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O ConfirmPassword é obrigatório")]
        [MinLength(4, ErrorMessage = "O campo Password deve possuir no mínimo 4 caracteres")]
        [MaxLength(20, ErrorMessage = "O campo Password deve possuir no máximo 20 caracteres")]
        [Compare("Password", ErrorMessage = "A senha não confere.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "O Token é obrigatório!")]
        public string Token { get; set; }
    }
}
