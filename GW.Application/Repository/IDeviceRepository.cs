using AutoMapper;
using GW.Application.Sevices;
using GW.Core.Context;
using GW.Core.Models;
using GW.Core.Models.Dto;
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
        public Result Insert(SettingDto setting);
        public Result Update(SettingDto setting);
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


        public Result Insert(SettingDto setting)
        {
            var check = !string.IsNullOrEmpty(setting.SerialNumber) ? _context.Devices
                .AsNoTracking()
                .Any(d => d.SerialNumber == setting.SerialNumber) : _context.Devices
                .AsNoTracking()
                .Any(d => d.BatchNumber == setting.BatchNumber);
            if (check) 
                return Result.Fail(ErrorCode.DUPLICATE_DATA, "سریال وارد شده قبلا ثبت شده است!");
            _context.Devices.Add(_mapper.Map<Device>(setting));
            _context.SaveChanges();
            return Result.Ok();
        }
        public Result Update(SettingDto setting)
        {
            var check = _context.Devices
                .Where(d=>d.SerialNumber==setting.SerialNumber)
                .FirstOrDefault();
            if (check is null)
                return Result.Fail(ErrorCode.DUPLICATE_DATA, "سریال وارد شده صحیح نیست!");

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
    }
}
