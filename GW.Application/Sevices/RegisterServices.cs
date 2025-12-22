using GW.Application.Mapper;
using GW.Application.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Application.Sevices
{
    public class RegisterServices
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBaseData, BaseData>();
            services.AddScoped<IOwnerRepository, OwnerRepository>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<ISoftwareVersionRepository, SoftwareRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IFOTARepository, FOTARepository>();


            services.AddAutoMapper(typeof(RoleProfile).Assembly);
        }
    }
}
