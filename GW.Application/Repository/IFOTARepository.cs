using AutoMapper;
using GW.Application.Sevices;
using GW.Core.Context;
using GW.Core.Models;
using GW.Core.Models.Dto;
using GW.Core.Models.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Application.Repository
{
    public interface IFOTARepository
    {
        public Task<Result> InsertAsync(UpdateFOTARequest fota);
        public Task<bool> Check(SettingDto setting);
    }

    public class FOTARepository : IFOTARepository
    {
        private SupervisorContext _context;
        private IMapper _mapper;
        private readonly IBaseData _baseData;

        #region ctor
        public FOTARepository(SupervisorContext context, IMapper mapper
            ,IBaseData baseData)
        {
            _context= context;
            _mapper= mapper;
            _baseData= baseData;
        }
        #endregion

        #region Insert
        public async Task<Result> InsertAsync(UpdateFOTARequest fota)
        {           
            var setting = JsonConvert.DeserializeObject<FOTADto>(fota.Settings);
            var path = await _baseData.PutFileAsync(fota.File);
            if (!string.IsNullOrEmpty(path))
            {
                FOTA fota_settings = _mapper.Map<FOTA>(setting);
                fota_settings.Path = path;
                
                _context.FOTA.Add(fota_settings);
                _context.SaveChanges();
                return Result.Ok();
            }
            return Result.Fail(ErrorCode.NO_FILE_UPLOADED, "خطا در آپلود فایل!");
        }
        #endregion

        #region Check
        public async Task<bool> Check(SettingDto setting)
        {
            return _context.FOTA
                .Any(f => f.Type == setting.Type
                && f.ProductCategory == setting.ProductCategory
                && f.ProductionDate.GetValueOrDefault().Date == setting.ProductionDate.Date
                && f.LastUpdate.GetValueOrDefault().Date == setting.LastUpdate.Date
                && f.FkOwnerId == setting.FkOwnerId
                && f.FkESPId == setting.FkESPId
                && f.FkHoltekId == setting.FkHoltekId
                && f.FkSTMId == setting.FkSTMId
                && f.BatchNumber == setting.BatchNumber
                && f.SerialNumber == setting.SerialNumber
                && f.IMEI == setting.IMEI
                && f.MAC == setting.MAC
                && f.HardwareVersion == setting.HardwareVersion);
        }
        #endregion
    }
}
