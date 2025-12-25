using GW.Core.Models.Enum;
using GW.Core.Models.Shared;
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
        public ProductCategory Category { get; set; }
        //todo category
        public string Path { get; set; }

    }

    public class UploadSoftwareVersion
    {
        [MaxLength(50)]
        public string Version { get; set; }
        public MicroType MicroType { get; set; }
        public DeviceType DeviceType { get; set; }
        public ProductCategory Category { get; set; }
    }

    public class RequestVersions
    {
        public DeviceType DeviceType { get; set; }
        public ProductCategory Category { get; set; }
    }

    public class ItemVersion
    {
        public int Id { get; set; }
        public string Version { get; set; }
    }

    public class CategorizedVersions
    {
        public List<ItemsListDto> ESP { get; set; } = new();
        public List<ItemsListDto> STM { get; set; } = new();
        public List<ItemsListDto> Holtek { get; set; } = new();
    }
}
