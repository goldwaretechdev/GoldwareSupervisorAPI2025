using AutoMapper;
using GW.Application.Sevices;
using GW.Core.Context;
using GW.Core.Models;
using GW.Core.Models.Dto;
using GW.Core.Models.Enum;
using GW.Core.Models.Shared;
using Microsoft.EntityFrameworkCore;

namespace GW.Application.Repository
{
    public interface IDeviceRepository
    {
        public Result<int> Insert(SettingDto setting, int userRoleId);
        public Result Update(SettingDto setting, int userRoleId);
        public Result<SettingDto> GetSettings(string serial);
        public Result<DeviceDto> GetDeviceByUniqueId(string id);
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
        public Result<int> Insert(SettingDto setting, int userRoleId)
        {
            try
            {
                var check = !string.IsNullOrEmpty(setting.SerialNumber) ? _context.Devices
               .AsNoTracking()
               .Any(d => d.SerialNumber == setting.SerialNumber) : _context.Devices
               .AsNoTracking()
               .Any(d => d.BatchNumber == setting.BatchNumber);
                if (check)
                    return Result<int>.Fail(ErrorCode.DUPLICATE_DATA, "سریال وارد شده قبلا ثبت شده است!");
                var device = _mapper.Map<Device>(setting);
                device.FkUserRoleId=userRoleId;

                _context.Devices.Add(device);
                _context.SaveChanges();

                return Result<int>.Ok(device.Id);
            }
            catch (Exception ex)
            {
                return Result<int>.Fail(ErrorCode.INTERNAL_ERROR, ex.Message);
            }
        }
        #endregion

        #region Update
        public Result Update(SettingDto setting, int userRoleId)
        {
            var check = _context.Devices
                .Where(d => d.SerialNumber == setting.SerialNumber)
                .FirstOrDefault();
            if (check is null)
                return Result.Fail(ErrorCode.NOT_FOUND, "سریال وارد شده صحیح نیست!");

            check.ProductCategory = setting.ProductCategory;
            check.FkOwnerId = setting.FkOwnerId;
            check.ProductionDate = setting.ProductionDate;
            check.BatchNumber = setting.BatchNumber;
            check.MAC = setting.MAC;
            check.IMEI = setting.IMEI;
            check.FkESPId = setting.FkESPId;
            check.FkHoltekId = setting.FkHoltekId;
            check.FkSTMId = setting.FkSTMId;
            check.LastUpdate = setting.LastUpdate;
            check.HardwareVersion = setting.HardwareVersion;
            check.Type = setting.Type;
            check.FkUserRoleId = userRoleId;
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
                .Include(d => d.ProductOwner)
                .Include(d => d.STM)
                .Include(d => d.ESP)
                .Include(d => d.Holtek)
                .Where(d => d.SerialNumber == serial)
                .FirstOrDefault();
            if (check is null)
                return Result<SettingDto>.Fail(ErrorCode.NOT_FOUND, "سریال وارد شده صحیح نیست!");
            result = _mapper.Map<SettingDto>(check);
            return Result<SettingDto>.Ok(result);
        }
        #endregion

        #region GetDeviceByUniqueId
        public Result<DeviceDto> GetDeviceByUniqueId(string id)
        {
            var device = _context.Devices
                .Where(d => d.UniqueId == id)
                .FirstOrDefault();
            if (device is null) return Result<DeviceDto>.Fail(ErrorCode.NOT_FOUND, "شناسه نامعتبر!");
            return Result<DeviceDto>.Ok(_mapper.Map<DeviceDto>(device));
        }
        #endregion
    }
}
