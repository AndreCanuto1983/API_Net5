using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Contracts.User
{
    public class UserRegisterInput
    {
        [Required(ErrorMessage = "O Email é obrigatório")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "O Phone é obrigatório")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "O Password é obrigatório")]
        [MinLength(6, ErrorMessage = "O Password deve possuir no mínimo 6 caracteres")]
        [MaxLength(20, ErrorMessage = "O Password deve possuir no máximo 20 caracteres")]        
        public string Password { get; set; }

        [Required(ErrorMessage = "O Name é obrigatório")]
        public string Name { get; set; }
        
        public string UserLocation { get; set; }

        [Required(ErrorMessage = "O Cpf é obrigatório")]
        [MinLength(11, ErrorMessage = "O campo Cpf deve possuir no mínimo 11 caracteres")]
        [MaxLength(11, ErrorMessage = "O campo Cpf deve possuir no máximo 11 caracteres")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O BirthDate é obrigatório")]
        public DateTime BirthDate { get; set; }
    }
}
