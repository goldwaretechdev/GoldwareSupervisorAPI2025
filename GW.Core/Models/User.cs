using System;
using System.ComponentModel.DataAnnotations;


namespace GW.Core.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Username { get; set; }
        [MaxLength(50)]
        public string FName { get; set; }
        [MaxLength(50)]
        public string LName { get; set; }
        [MinLength(8), MaxLength(100)]
        public string Password { get; set; }
        [MaxLength(15)]
        public string Mobile { get; set; }


        public ICollection<UserRoles> UserRoles { get; set; }
        public ICollection<UserAndCompany> UserAndCompanies { get; set; }
        public ICollection<Access> Access { get; set; }

    }
}
