using AutoMapper;
using GW.Application.Sevices;
using GW.Core.Context;
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
        public List<SelectListItem> Owners();
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


        public List<SelectListItem> Owners()
        {
            return _context.Company
                .AsNoTracking()
                .Select(c => new SelectListItem
                {
                   Value=  c.Id.ToString(),
                    Text= c.Name
                }).ToList();
        }
    }
}
