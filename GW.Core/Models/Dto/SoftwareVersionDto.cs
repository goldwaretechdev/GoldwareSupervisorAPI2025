using GW.Core.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Core.Models.Dto
{
    public class SoftwareVersionDto
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Version { get; set; }
        public MicroType MicroType { get; set; }
        public DeviceType DeviceType { get; set; }
        //todo category
        public string Path { get; set; }

    }

    public class ItemVersion
    {
        public int Id { get; set; }
        public string Version { get; set; }
    }

    public class CategorizedVersions
    {
        public List<ItemVersion> ESP { get; set; }
        public List<ItemVersion> STM { get; set; }
        public List<ItemVersion> Holtek { get; set; }
    }
}
