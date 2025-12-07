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
        [MaxLength(50)]
        public string Version { get; set; }
        public MicroType MicroType { get; set; }
        public DeviceType DeviceType { get; set; }
        public string Path { get; set; }


        public ICollection<Device> ESPVersions { get; set; }
        public ICollection<Device> STMVersions { get; set; }
        public ICollection<Device> HoltekVersions { get; set; }

        public ICollection<FOTA> FOTAESPVersions { get; set; }
        public ICollection<FOTA> FOTASTMVersions { get; set; }
        public ICollection<FOTA> FOTAHoltekVersions { get; set; }
    }
}
