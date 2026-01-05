using GW.Core.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Core.Models
{
    public class SoftwareVersion
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Version { get; set; }
        public MicroType MicroType { get; set; }
        public DeviceType DeviceType { get; set; }
        public ProductCategory Category { get; set; }
        public string Path { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        [MaxLength(15)]
        public string MinHardwareVersion { get; set; }
        [MaxLength(15)]
        public string MaxHardwareVersion { get; set; }
        public bool IsActive { get; set; } = true;

        public UserRoles UserRoles { get; set; }
        public int FkUserRoleId { get; set; }


        public ICollection<Device> ESPVersions { get; set; }
        public ICollection<Device> STMVersions { get; set; }
        public ICollection<Device> HoltekVersions { get; set; }

        public ICollection<FOTA> FOTAESPVersions { get; set; }
        public ICollection<FOTA> FOTASTMVersions { get; set; }
        public ICollection<FOTA> FOTAHoltekVersions { get; set; }
    }
}
