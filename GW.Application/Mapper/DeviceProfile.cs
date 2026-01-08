using AutoMapper;
using GW.Core.Models;
using GW.Core.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Application.Mapper
{
    public class DeviceProfile:Profile
    {
        public DeviceProfile()
        {
            CreateMap<SettingDto, Device>();
            //CreateMap<Device, SettingDto>()
            //.ForPath(dest => dest.OwnerName,
            //    opt => opt.MapFrom(src => src.ProductOwner.Name))
            //.ForPath(dest => dest.OwnerName,
            //    opt => opt.MapFrom(src => src.ProductOwner.Name))
            //.ForPath(dest => dest.ESPVersion,
            //    opt => opt.MapFrom(src => src.ESP != null ? src.ESP.Version : string.Empty))
            //.ForPath(dest => dest.STMVersion,
            //    opt => opt.MapFrom(src => src.STM != null ? src.STM.Version : string.Empty))
            //.ForPath(dest => dest.HoltekVersion,
            //    opt => opt.MapFrom(src => src.Holtek != null ? src.Holtek.Version : string.Empty));
            CreateMap<Device, DeviceDto>();
                
        }
    }
}
