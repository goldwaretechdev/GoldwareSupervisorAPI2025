
using System.ComponentModel.DataAnnotations;


namespace GW.Core.Models
{
    public class UserRoles
    {
        [Key]
        public int Id { get; set; }

        public User User { get; set; }
        public Guid FkUserId { get; set; }

        public Role Role { get; set; }
        public int FkRoleId { get; set; }

        public ICollection<Log> Logs{ get; set; }
        public ICollection<SoftwareVersion> SoftwareVersions { get; set; }
        public ICollection<FOTA> FOTAs { get; set; }
    }
}
