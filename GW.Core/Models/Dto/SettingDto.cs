using GW.Core.Models.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace GW.Core.Models.Dto
{
    public class SettingDto
    {
        public DeviceType Type { get; set; }                                         //100                
        public ProductCategory ProductCategory { get; set; }                         //101
        [MaxLength(50)]
        public string? BatchNumber { get; set; }                                     //102
        [MaxLength(50)]
        public string? SerialNumber { get; set; }                                    //103
        public DateTime ProductionDate { get; set; }                                 //104
        public DateTime LastUpdate { get; set; }                                     //105
        [MaxLength(50)]
        public string HardwareVersion { get; set; }                                  //106
        [MaxLength(50)]
        public string? MAC { get; set; }                                             //107
        [MaxLength(50)]
        public string? IMEI { get; set; }                                            //108
        public int FkOwnerId { get; set; }                                           //109
        public string OwnerName { get; set; }                                        //110
        public int FkESPId { get; set; }                                             //111
        public string ESPVersion { get; set; }                                       //112
        public int FkSTMId { get; set; }                                             //113
        public string STMVersion { get; set; }                                       //114
        public int FkHoltekId { get; set; }                                          //115
        public string HoltekVersion { get; set; }                                    //116
    }

    public class UpdateFOTARequest
    {
        public string Settings { get; set; }
        public IFormFile File { get; set; }
    }
}
