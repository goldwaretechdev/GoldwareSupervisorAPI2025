using GW.Core.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Core.Models
{
    public class FOTA
    {
        [Key]
        public int Id { get; set; }
        public DeviceType Type { get; set; }
        public string Path { get; set; }
        public ProductCategory? ProductCategory { get; set; }
        [MaxLength(50)]
        public string? BatchNumber { get; set; }
        [MaxLength(50)]
        public string? SerialNumber { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime? ProductionDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        [MaxLength(50)]
        public string? HardwareVersion { get; set; }
        [MaxLength(50)]
        public string? MAC { get; set; }
        [MaxLength(50)]
        public string? IMEI { get; set; }

        public Company? ProductOwner { get; set; }
        public int? FkOwnerId { get; set; }

        public SoftwareVersion? ESP { get; set; }
        public int? FkESPId { get; set; }

        public SoftwareVersion? STM { get; set; }
        public int? FkSTMId { get; set; }

        public SoftwareVersion? Holtek { get; set; }
        public int? FkHoltekId { get; set; }
    }
}
