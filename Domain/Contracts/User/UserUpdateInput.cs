using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.User
{
    public class UserUpdateInput
    {
        [Required(ErrorMessage = "O Email é obrigatório")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "O Phone é obrigatório")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

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