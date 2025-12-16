using AutoMapper;
using GW.Application.Sevices;
using GW.Core.Context;
using GW.Core.Models.Dto;
using GW.Core.Models.Enum;
using GW.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Application.Repository
{
    public interface ISoftwareVersionRepository
    {
        public Result<CategorizedVersions> CategorizedVersions(RequestVersions request);
    }


    public class SoftwareRepository : ISoftwareVersionRepository
    {
        private readonly IBaseData _baseData;
        private SupervisorContext _context;
        private readonly IMapper _mapper;

        public SoftwareRepository(SupervisorContext context, IMapper mapper, IBaseData baseData)
        {
            _context = context;
            _mapper = mapper;
            _baseData = baseData;
        }

        public Result<CategorizedVersions> CategorizedVersions(RequestVersions request)
        {
            var data = _context.SoftwareVersions
                .Where(s=>s.Category==request.Category && s.DeviceType==request.DeviceType)
                .Select(s=>new {s.Id,s.Version,s.MicroType})
                .ToList();
            CategorizedVersions categorized = new();
            foreach (var item in data)
            {
                switch (item.MicroType)
                {
                    case MicroType.Holtek:
                        categorized.Holtek.Add(new() { Value = item.Id,Text =item.Version});
                        break;
                    case MicroType.STM:
                        categorized.STM.Add(new() { Value = item.Id, Text = item.Version });
                        break;
                    case MicroType.ESP:
                        categorized.ESP.Add(new() { Value = item.Id, Text = item.Version });
                        break;
                }
            }
            return Result<CategorizedVersions>.Ok(categorized);
        }
    }
}
