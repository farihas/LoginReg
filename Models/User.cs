using System;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;

namespace LoginReg.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        public string FirstName { get; set;}
        
        [Required]
        [MinLength(2)]
        public string LastName { get; set;}

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [NotMapped]
        public string Password { get; set; }

        [Required]
        [NotMapped]
        public string PwConfirm { get; set; }  

        public string PasswordHash { get; set; }  

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created_at { get; set; }   

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Updated_at { get; set; }
    }
}