using AutoMapper;
using GW.Core.Context;
using GW.Core.Models.Dto;
using GW.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Application.Repository
{
    public interface IFOTARepository
    {
        public Result Insert(UpdateFOTARequest fota);
    }

    public class FOTARepository : IFOTARepository
    {
        private SupervisorContext _context;
        private IMapper _mapper;

        #region ctor
        public FOTARepository(SupervisorContext context, IMapper mapper)
        {
            _context= context;
            _mapper= mapper;
        }
        #endregion

        #region Insert
        public Result Insert(UpdateFOTARequest fota)
        {

            return Result.Ok();
        }
        #endregion
    }
}
