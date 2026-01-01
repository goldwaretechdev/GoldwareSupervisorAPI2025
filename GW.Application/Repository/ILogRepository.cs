using AutoMapper;
using GW.Core.Context;
using GW.Core.Models;
using GW.Core.Models.Dto;
using GW.Core.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Application.Repository
{
    public interface ILogRepository
    {
        public Task<Result> Insert(LogDto log);
    }



    public class LogRepository : ILogRepository
    {
        private SupervisorContext _context;
        private readonly IMapper _mapper;

        #region ctor
        public LogRepository(SupervisorContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Insert
        public async Task<Result> Insert(LogDto log)
        {
            _context.Logs.Add(_mapper.Map<Log>(log));
            _context.SaveChanges();
            return Result.Ok();
        }
        #endregion
    }
}
