using GW.Application.Repository;
using GW.Core.Models.Dto;
using GW.Core.Models.Enum;
using GW.Core.Models.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Application.Sevices
{
    public interface ISettingsService
    {
        public Result<BaseInfo> Info();
    }


    public class SettingsService : ISettingsService
    {
        private readonly IOwnerRepository _ownerRepository;
        public SettingsService(IOwnerRepository ownerRepository)
        {
            _ownerRepository= ownerRepository;
        }

        public Result<BaseInfo> Info()
        {
            BaseInfo response = new();

            response.DeviceTypes = Enum.GetValues(typeof(DeviceType))
           .Cast<DeviceType>()
           .Select(e => new SelectListItem
           {
               Value = Convert.ToInt32(e).ToString(),
               Text = e.ToString()
           })
           .ToList();

            response.ProductCategory = Enum.GetValues(typeof(ProductCategory))
           .Cast<ProductCategory>()
           .Select(e => new SelectListItem
           {
               Value = Convert.ToInt32(e).ToString(),
               Text = e.ToString()
           })
           .ToList();
            response.ProductOwner = _ownerRepository.Owners();

            return Result<BaseInfo>.Ok(response);
        }
    }
}
