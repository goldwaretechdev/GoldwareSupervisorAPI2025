using System;
using System.ComponentModel.DataAnnotations;


namespace GW.Core.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
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

        public Company Company { get; set; }
        public int FkCompanyId { get; set; }

        public ICollection<UserRoles> UserRoles { get; set; }


    }
}
