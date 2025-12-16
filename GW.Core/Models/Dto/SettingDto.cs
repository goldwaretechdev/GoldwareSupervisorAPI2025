using GW.Core.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace GW.Core.Models.Dto
{
    public class SettingDto
    {
        public DeviceType Type { get; set; }
        public ProductCategory ProductCategory { get; set; }
        [MaxLength(50)]
        public string? BatchNumber { get; set; }
        [MaxLength(50)]
        public string? SerialNumber { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime LastUpdate { get; set; }
        [MaxLength(50)]
        public string HardwareVersion { get; set; }
        [MaxLength(50)]
        public string? MAC { get; set; }
        [MaxLength(50)]
        public string? IMEI { get; set; }
        public int FkOwnerId { get; set; }
        public int FkESPId { get; set; }
        public int FkSTMId { get; set; }
        public int FkHoltekId { get; set; }
    }

    //public class UpdateDeviceSettings:SettingDto
    //{
    //    public int Id { get; set; }
    //}
}
