using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class UserModel : IdentityUser
    {
        public UserModel()
        {
            CreationDate = DateTime.UtcNow;
            UpdateDate = CreationDate;
        }

        public string Name { get; set; }
        public string UserLocation { get; set; }
        public string Cpf { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CreationDate { get; set; }        
        [Required]
        public DateTime UpdateDate { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        
    }
}
