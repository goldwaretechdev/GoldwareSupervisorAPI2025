using AutoMapper;
using GW.Application.Sevices;
using GW.Core.Context;
using GW.Core.Models;
using GW.Core.Models.Dto;
using GW.Core.Models.Enum;
using GW.Core.Models.Shared;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GW.Application.Repository
{
    public interface ISoftwareVersionRepository
    {
        public Result<CategorizedVersions> CategorizedVersions(RequestVersions request);
        public string File(int id);
        public Task<Result> Insert(UploadSoftwareVersion version,int userRoleId);
    }


    public class SoftwareRepository : ISoftwareVersionRepository
    {
        private readonly IBaseData _baseData;
        private SupervisorContext _context;
        private readonly IMapper _mapper;

        #region ctor
        public SoftwareRepository(SupervisorContext context, IMapper mapper, IBaseData baseData)
        {
            _context = context;
            _mapper = mapper;
            _baseData = baseData;
        }
        #endregion

        #region CategorizedVersions
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
        #endregion

        #region File
        public string File(int id)
        {
            var softwareVersion = _context.SoftwareVersions
            .Where(s => s.Id == id).FirstOrDefault();

            if (softwareVersion is null) return string.Empty;
           
            return softwareVersion.Path ?? string.Empty;
        }
        #endregion

        #region Insert
        public async Task<Result> Insert(UploadSoftwareVersion version, int userRoleId)
        {
            var condition = JsonConvert.DeserializeObject<VersionConditions>(version.Condition);
            if (_context.SoftwareVersions
                .Any(s => s.DeviceType == condition.DeviceType
                && s.Category == condition.Category
                && s.MicroType == condition.MicroType
                && s.Version == condition.Version))
                return Result.Fail(ErrorCode.DUPLICATE_DATA, "ورژن وارد شده تکراری می باشد!");
            var path = await _baseData.PutFileAsync(version.File,Constants.SOFT_FILE);
            if (!string.IsNullOrEmpty(path))
            {
                SoftwareVersion softwareVersion = new()
                {
                    DeviceType=condition.DeviceType,
                    Category = condition.Category,
                    MicroType = condition.MicroType,
                    DateTime = DateTime.Now,
                    Version =condition.Version,
                    Path = path,
                    FkUserRoleId = userRoleId
                };
                _context.SoftwareVersions.Add(softwareVersion);
                _context.SaveChanges();
                return Result.Ok();
            }
            return Result.Fail(ErrorCode.NO_FILE_UPLOADED, "خطا در آپلود فایل!");
        }

        #endregion
    }
}
