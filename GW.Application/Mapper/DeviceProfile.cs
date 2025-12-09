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
        }
    }
}
