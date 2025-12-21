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
            CreateMap<Device, SettingDto>()
                .ForPath(dst => dst.OwnerName, opt =>
                opt.MapFrom(src => src.ProductOwner.Name))
                .ForPath(dst => dst.ESPVersion, opt =>
                opt.MapFrom(src => src.ESP.Version))
                .ForPath(dst => dst.STMVersion, opt =>
                opt.MapFrom(src => src.STM.Version))
                .ForPath(dst => dst.HoltekVersion, opt =>
                opt.MapFrom(src => src.Holtek.Version));
                
        }
    }
}
