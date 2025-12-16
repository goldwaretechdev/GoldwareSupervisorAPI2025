using AutoMapper;
using GW.Application.Sevices;
using GW.Core.Context;
using GW.Core.Models.Dto;
using GW.Core.Models.Enum;
using GW.Core.Models.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Application.Repository
{
    public interface IOwnerRepository
    {
        public Result<List<ItemsListDto>> Owners();
        //public Result<BaseInfo> Info();

    }

    public class OwnerRepository : IOwnerRepository
    {
        private readonly IBaseData _baseData;
        private SupervisorContext _context;
        private readonly IMapper _mapper;

        public OwnerRepository(SupervisorContext context, IMapper mapper, IBaseData baseData)
        {
            _context = context;
            _mapper = mapper;
            _baseData = baseData;
        }


        public Result<List<ItemsListDto>> Owners()
        {
            var result = _context.Company
                .AsNoTracking()
                .Select(c => new ItemsListDto
                {
                   Value=  c.Id,
                    Text= c.Name
                }).ToList();
            return Result<List<ItemsListDto>>.Ok(result);
        }

        //public Result<BaseInfo> Info()
        //{
        //    BaseInfo response = new();

        //    response.DeviceTypes = Enum.GetValues(typeof(DeviceType))
        //   .Cast<DeviceType>()
        //   .Select(e => new SelectListItem
        //   {
        //       Value = Convert.ToInt32(e).ToString(),
        //       Text = e.ToString()
        //   })
        //   .ToList();

        //    response.ProductCategory = Enum.GetValues(typeof(ProductCategory))
        //   .Cast<ProductCategory>()
        //   .Select(e => new SelectListItem
        //   {
        //       Value = Convert.ToInt32(e).ToString(),
        //       Text = e.ToString()
        //   })
        //   .ToList();
        //    response.ProductOwner = Owners();

        //    return Result<BaseInfo>.Ok(response);
        //}

    }
}
