using AutoMapper;
using GW.Application.Sevices;
using GW.Core.Context;
using GW.Core.Models;
using GW.Core.Models.Dto;
using GW.Core.Models.Enum;
using GW.Core.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Application.Repository
{
    public interface IDeviceRepository
    {
        public Result Insert(SettingDto setting,int userRoleId);
        public Result Update(SettingDto setting);
        public Result<SettingDto> GetSettings(string serial);
    }

    public class DeviceRepository : IDeviceRepository
    {
        private readonly IBaseData _baseData;
        private SupervisorContext _context;
        private readonly IMapper _mapper;

        public DeviceRepository(SupervisorContext context, IMapper mapper, IBaseData baseData)
        {
            _context = context;
            _mapper = mapper;
            _baseData = baseData;
        }

        #region Insert
        public Result Insert(SettingDto setting,int userRoleId)
        {
            try
            {
                _context.Database.BeginTransaction();
                var check = !string.IsNullOrEmpty(setting.SerialNumber) ? _context.Devices
               .AsNoTracking()
               .Any(d => d.SerialNumber == setting.SerialNumber) : _context.Devices
               .AsNoTracking()
               .Any(d => d.BatchNumber == setting.BatchNumber);
                if (check)
                    return Result.Fail(ErrorCode.DUPLICATE_DATA, "سریال وارد شده قبلا ثبت شده است!");
                var device = _mapper.Map<Device>(setting);
                _context.Devices.Add(device);
                _context.SaveChanges();
                //save log
                Log log = new()
                {
                    DateTime = DateTime.Now,
                    FkDeviceId = device.Id,
                    FkUserRoleId = userRoleId,
                    Type = LogType.SetSettings,
                };
                _context.Logs.Add(log);
                _context.SaveChanges();

                _context.Database.CommitTransaction();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                return Result.Fail(ErrorCode.INTERNAL_ERROR, ex.Message);
            }
        }
        #endregion

        #region Update
        public Result Update(SettingDto setting)
        {
            var check = _context.Devices
                .Where(d=>d.SerialNumber==setting.SerialNumber)
                .FirstOrDefault();
            if (check is null)
                return Result.Fail(ErrorCode.NOT_FOUND, "سریال وارد شده صحیح نیست!");

            check.ProductCategory= setting.ProductCategory;
            check.FkOwnerId= setting.FkOwnerId;
            check.ProductionDate= setting.ProductionDate;
            check.BatchNumber= setting.BatchNumber;
            check.MAC= setting.MAC;
            check.IMEI= setting.IMEI;
            check.FkESPId= setting.FkESPId;
            check.FkHoltekId= setting.FkHoltekId;
            check.FkSTMId= setting.FkSTMId;
            check.LastUpdate= setting.LastUpdate;
            check.HardwareVersion= setting.HardwareVersion;
            check.Type= setting.Type;

            _context.Devices.Update(_mapper.Map<Device>(check));
            _context.SaveChanges();
            return Result.Ok();
        }
        #endregion

        #region GetSettings
        public Result<SettingDto> GetSettings(string serial)
        {
            SettingDto result = new();
            var check = _context.Devices
                .AsNoTracking()
                .Include(d=>d.ProductOwner)
                .Include(d=>d.STM)
                .Include(d=>d.ESP)
                .Include(d=>d.Holtek)
                .Where(d=>d.SerialNumber==serial)
                .FirstOrDefault();
            if (check is null)
                return Result<SettingDto>.Fail(ErrorCode.NOT_FOUND, "سریال وارد شده صحیح نیست!");
           result = _mapper.Map<SettingDto>(check); 
            return Result<SettingDto>.Ok(result);
        }
        #endregion
    }
}
