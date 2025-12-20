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
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UserRoles, UserRoleDto>()
                .ForPath(dst => dst.Role, opt =>
                opt.MapFrom(src => src.Role.Name));
        }
    }
}
