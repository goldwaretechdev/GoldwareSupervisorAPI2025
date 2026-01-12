using AutoMapper;
using GW.Application.Sevices;
using GW.Core.Context;
using GW.Core.Models;
using GW.Core.Models.Dto;
using GW.Core.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GW.Application.Repository
{
    public interface IFOTARepository
    {
        public Task<Result> InsertAsync(FOTADto fota, Guid userId);
        public Task<Result> InsertAndDeactiveSameFiles(FOTADto fota, int userRole);
        public FOTADto Check(FOTADto setting);
        public Result<List<FOTADto>> All(UserDto user);
    }

    public class FOTARepository : IFOTARepository
    {
        private SupervisorContext _context;
        private IMapper _mapper;
        private readonly IBaseData _baseData;

        #region ctor
        public FOTARepository(SupervisorContext context, IMapper mapper
            , IBaseData baseData)
        {
            _context = context;
            _mapper = mapper;
            _baseData = baseData;
        }
        #endregion

        #region Insert
        public async Task<Result> InsertAsync(FOTADto fota, Guid userId)
        {
            var check = _context.FOTA
               .Where(f =>
                   f.Type == fota.Type &&
                   f.ProductCategory == fota.ProductCategory &&
                   f.FkOwnerId == fota.FkOwnerId &&
                   f.FkESPId == fota.FkESPId &&
                   f.FkHoltekId == fota.FkHoltekId &&
                   f.FkSTMId == fota.FkSTMId &&
                   f.BatchNumber == fota.BatchNumber &&
                   f.SerialNumber == fota.SerialNumber &&
                   f.IMEI == fota.IMEI &&
                   f.MAC == fota.MAC &&
                   f.HardwareVersion == fota.HardwareVersion &&
                   f.IsActive
               )
               .FirstOrDefault();
            if (check is not null)
            {
                if (check.ProductionDate.GetValueOrDefault().Date == fota.ProductionDate.GetValueOrDefault().Date
                && check.LastUpdate.GetValueOrDefault().Date == fota.LastUpdate.GetValueOrDefault().Date)
                {
                    //string name = check.Path.Split("\\").Last();
                    var softwareVersion = _context.SoftwareVersions.Where(s => fota.FkESPId != null ?
                    s.Id == fota.FkESPId : fota.FkSTMId != null ?
                    s.Id == fota.FkSTMId : s.Id == fota.FkHoltekId).FirstOrDefault();
                    if (softwareVersion is null) return Result.Fail(ErrorCode.INVALID_ID, "شناسه نامعتبر!");
                    check.Path = softwareVersion.Path;
                    _context.FOTA.Update(check);
                    _context.SaveChanges();
                    return Result.Ok();
                }
                else
                {
                    return Result.Fail(ErrorCode.SAME_FILE_EXISTS, "FOTA با شرایط مشابه وجود دارد.");
                }
            }
            else
            {
                var check_same = _context.FOTA
               .Where(f =>
                  (f.Type == fota.Type ||
                   f.ProductCategory == fota.ProductCategory ||
                   f.FkOwnerId == fota.FkOwnerId ||
                   f.FkESPId == fota.FkESPId ||
                   f.FkHoltekId == fota.FkHoltekId ||
                   f.FkSTMId == fota.FkSTMId ||
                   f.BatchNumber == fota.BatchNumber ||
                   f.SerialNumber == fota.SerialNumber ||
                   f.IMEI == fota.IMEI ||
                   f.MAC == fota.MAC ||
                   f.HardwareVersion == fota.HardwareVersion) &&
                   f.IsActive)
               .FirstOrDefault();
                if (check_same is not null)
                    return Result.Fail(ErrorCode.SAME_FILE_EXISTS, "FOTA با شرایط مشابه وجود دارد.");


                var softwareVersion = _context.SoftwareVersions.Where(s => fota.FkESPId != null ?
                    s.Id == fota.FkESPId : fota.FkSTMId != null ?
                    s.Id == fota.FkSTMId : s.Id == fota.FkHoltekId).FirstOrDefault();
                if (softwareVersion is null) return Result.Fail(ErrorCode.INVALID_ID, "شناسه نامعتبر!");
                if (!string.IsNullOrEmpty(softwareVersion.Path))
                {
                    var userRole = _context.UserRoles
                    .AsNoTracking()
                    .Include(u => u.User)
               .Where(u => u.FkUserId == userId).FirstOrDefault();
                    if (userRole is null) return Result<int>.Fail(ErrorCode.NOT_FOUND, "کاربر غیرمجاز!");
                    //FOTA fota_settings = _mapper.Map<FOTA>(setting);
                    FOTA fota_settings = new()
                    {
                        BatchNumber = fota.BatchNumber,
                        SerialNumber = fota.SerialNumber,
                        IMEI = fota.IMEI,
                        MAC = fota.MAC,
                        ExpireDate = fota.ExpireDate,
                        ProductCategory = fota.ProductCategory,
                        Type = fota.Type,
                        ProductionDate = fota.ProductionDate,
                        LastUpdate = fota.LastUpdate,
                        FkESPId = fota.FkESPId,
                        FkHoltekId = fota.FkHoltekId,
                        FkOwnerId = fota.FkOwnerId,
                        FkMainOwnerId = userRole.User.FkCompanyId,
                        FkSTMId = fota.FkSTMId,
                        HardwareVersion = fota.HardwareVersion,
                        Path = softwareVersion.Path,
                        FkUserRoleId = userRole.Id,
                        IsActive = true
                    };
                    _context.FOTA.Add(fota_settings);
                    _context.SaveChanges();
                    return Result.Ok();
                }
                return Result.Fail(ErrorCode.NO_FILE_UPLOADED, "خطا در ثبت اطلاعات!");
            }
        }
        #endregion

        #region InsertAndDeactiveSameFiles
        public async Task<Result> InsertAndDeactiveSameFiles(FOTADto fota, int userRole)
        {
            _context.Database.BeginTransaction();
            try
            {
                var check_same = _context.FOTA
               .Where(f =>
                  (f.Type == fota.Type ||
                   f.ProductCategory == fota.ProductCategory ||
                   f.FkOwnerId == fota.FkOwnerId ||
                   f.FkESPId == fota.FkESPId ||
                   f.FkHoltekId == fota.FkHoltekId ||
                   f.FkSTMId == fota.FkSTMId ||
                   f.BatchNumber == fota.BatchNumber ||
                   f.SerialNumber == fota.SerialNumber ||
                   f.IMEI == fota.IMEI ||
                   f.MAC == fota.MAC ||
                   f.HardwareVersion == fota.HardwareVersion) &&
                   f.IsActive)
               .ToList();
                foreach (var item in check_same)
                {
                    item.IsActive = false;
                    _context.FOTA.Update(item);
                }

                //string name = Guid.NewGuid().ToString() + Path.GetExtension(fota.File.FileName);
                //var path = await _baseData.PutFileAsync(fota.File, Constants.FOTA_FILE, name);
                var softwareVersion = _context.SoftwareVersions.Where(s => fota.FkESPId != null ?
                   s.Id == fota.FkESPId : fota.FkSTMId != null ?
                   s.Id == fota.FkSTMId : s.Id == fota.FkHoltekId).FirstOrDefault();
                if (softwareVersion is null) return Result.Fail(ErrorCode.INVALID_ID, "شناسه نامعتبر!");
                if (!string.IsNullOrEmpty(softwareVersion.Path))
                {
                    //FOTA fota_settings = _mapper.Map<FOTA>(setting);
                    FOTA fota_settings = new()
                    {
                        BatchNumber = fota.BatchNumber,
                        SerialNumber = fota.SerialNumber,
                        IMEI = fota.IMEI,
                        MAC = fota.MAC,
                        ExpireDate = fota.ExpireDate,
                        ProductCategory = fota.ProductCategory,
                        Type = fota.Type,
                        ProductionDate = fota.ProductionDate,
                        LastUpdate = fota.LastUpdate,
                        FkESPId = fota.FkESPId,
                        FkHoltekId = fota.FkHoltekId,
                        FkOwnerId = fota.FkOwnerId,
                        FkSTMId = fota.FkSTMId,
                        HardwareVersion = fota.HardwareVersion,
                        Path = softwareVersion.Path,
                        FkUserRoleId = userRole,
                        IsActive = true,
                    };
                    _context.FOTA.Add(fota_settings);
                    _context.SaveChanges();
                    _context.Database.CommitTransaction();
                    return Result.Ok();
                }
                _context.Database.RollbackTransaction();
                return Result.Fail(ErrorCode.NO_FILE_UPLOADED, "خطا در ثبت اطلاعات!");
            }
            catch (Exception)
            {
                _context.Database.RollbackTransaction();
                return Result.Fail(ErrorCode.INTERNAL_ERROR, "خطای سرور!");

            }
        }
        #endregion

        #region Check
        public FOTADto? Check(FOTADto setting)
        {
            var softwareVersion = _context.SoftwareVersions.Where(s => setting.FkESPId != null ?
                              s.Id == setting.FkESPId : setting.FkSTMId != null ?
                              s.Id == setting.FkSTMId : s.Id == setting.FkHoltekId).FirstOrDefault();
            if (softwareVersion is null) return null;
            var fota = _context.FOTA
                .Where(f =>
                    f.Type == setting.Type &&
                    (f.ProductCategory == null || f.ProductCategory == setting.ProductCategory) &&
                    (f.FkOwnerId == null || f.FkOwnerId == setting.FkOwnerId) &&
                    ((f.FkESPId != null && f.FkESPId != setting.FkESPId) ||
                    (f.FkHoltekId != null && f.FkHoltekId != setting.FkHoltekId) ||
                    (f.FkSTMId != null && f.FkSTMId != setting.FkSTMId)) &&
                    (f.BatchNumber == null || f.BatchNumber == setting.BatchNumber) &&
                    (f.SerialNumber == null || f.SerialNumber == setting.SerialNumber) &&
                    (f.IMEI == null || f.IMEI == setting.IMEI) &&
                    (f.MAC == null || f.MAC == setting.MAC)
                    //(f.HardwareVersion == null || f.HardwareVersion == setting.HardwareVersion)
                    && f.IsActive)
                .FirstOrDefault();
            if (fota is not null)
            {
                if ((fota.ProductionDate == null || fota.ProductionDate.GetValueOrDefault().Date != setting.ProductionDate.GetValueOrDefault().Date)
                && (fota.LastUpdate == null || fota.LastUpdate.GetValueOrDefault().Date != setting.LastUpdate.GetValueOrDefault().Date))
                    return null;
                var hardware = fota.HardwareVersion.Split(".");
                var min_hardware = softwareVersion.MinHardwareVersion.Split(".");
                var max_hardware = softwareVersion.MaxHardwareVersion.Split(".");
                if (!(int.Parse(hardware[1]) > int.Parse(min_hardware[1]) && int.Parse(hardware[1]) < int.Parse(max_hardware[1]) &&
                    int.Parse(hardware[2]) > int.Parse(min_hardware[2]) && int.Parse(hardware[2]) < int.Parse(max_hardware[2]) &&
                    int.Parse(hardware[3]) > int.Parse(min_hardware[3]) && int.Parse(hardware[3]) < int.Parse(max_hardware[3])
                    ))
                    return null;

            }
            else { return null; }
            return _mapper.Map<FOTADto>(fota);
        }

        #endregion

        #region All
        public Result<List<FOTADto>> All(UserDto user)
        {
            List<FOTADto> result = new();
            var check = _context.FOTA
                .AsNoTracking()
                .Include(d => d.ProductOwner)
                .Include(d => d.STM)
                .Include(d => d.ESP)
                .Include(d => d.Holtek)
                .Where(d => d.FkMainOwnerId == user.FkCompanyId)
                .OrderByDescending(d => d.ProductionDate)
                .Take(20);
            foreach (var item in check)
                result.Add(new()
                {
                    BatchNumber = item.BatchNumber,
                    SerialNumber = item.SerialNumber,
                    ProductCategory = item.ProductCategory,
                    Type = item.Type,
                    ProductionDate = item.ProductionDate,
                    LastUpdate = item.LastUpdate,
                    FkOwnerId = item.FkOwnerId,
                    OwnerName = item.ProductOwner.Name,
                    FkESPId = item.FkESPId,
                    ESPVersion = item.ESP?.Version,
                    FkHoltekId = item.FkHoltekId,
                    HoltekVersion = item.Holtek?.Version,
                    FkSTMId = item.FkSTMId,
                    STMVersion = item.STM?.Version,
                    HardwareVersion = item.HardwareVersion,
                    MAC = item.MAC,
                    IMEI = item.IMEI,
                });
            return Result<List<FOTADto>>.Ok(result);
        }
        #endregion
    }
}
