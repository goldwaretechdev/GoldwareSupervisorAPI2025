
using System.ComponentModel.DataAnnotations;


namespace GW.Core.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<UserRoles> UserRoles { get; set; }

    }
}
