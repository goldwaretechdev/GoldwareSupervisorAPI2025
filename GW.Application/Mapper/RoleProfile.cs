using AutoMapper;
using GW.Core.Models;
using GW.Core.Models.Dto;

namespace GW.Application.Mapper
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>()
                .ForMember(dst => dst.Text, opt =>
                opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Value, opt =>
                opt.MapFrom(src => src.Id));
        }
    }
}
