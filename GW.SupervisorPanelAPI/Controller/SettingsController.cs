using Azure.Core;
using GW.Application.Repository;
using GW.Application.Sevices;
using GW.Core.Models;
using GW.Core.Models.Dto;
using GW.Core.Models.Enum;
using GW.Core.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;

namespace GW.SupervisorPanelAPI.Controller
{
    [Route("api/[controller]/[action]" )]
    [ApiController]
    [Authorize(Roles ="Development")]
    public class SettingsController : ControllerBase
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly ILogRepository _logRepository;
        private readonly IFOTARepository _fotaRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBaseData _baseData;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ISoftwareVersionRepository _softwareVersionRepository;
        private readonly ILogger<SettingsController> _logger;

        #region ctor
        public SettingsController(IDeviceRepository deviceRepository,IBaseData baseData
            ,IOwnerRepository ownerRepository
            ,ISoftwareVersionRepository softwareVersion,IUserRepository userRepository
            ,IFOTARepository fOTARepository
            ,ILogRepository logRepository,
            ILogger<SettingsController> logger)
        {
            _deviceRepository = deviceRepository;
            _userRepository = userRepository;
            _baseData = baseData;
            _ownerRepository = ownerRepository;
            _softwareVersionRepository = softwareVersion;
            _fotaRepository = fOTARepository;
            _logRepository = logRepository;
            _logger= logger;
        }
        #endregion

        #region Owners
        [HttpGet]
        public IActionResult Owners()
        {
            try
            {
                var result = _ownerRepository.Owners();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Owners Failed");
                return BadRequest(new { ErrorCode.INTERNAL_ERROR, ex.Message });
            }
        }
        #endregion

        #region SoftwareVersions
        [HttpPost]
        public IActionResult SoftwareVersions([FromBody]RequestVersions request)
        {
            try
            {
                var result = _softwareVersionRepository.CategorizedVersions(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get SoftwareVersions Failed by input: {request}", request);
                return BadRequest(new { ErrorCode.INTERNAL_ERROR, ex.Message });
            }
        }
        #endregion

        #region SetDeviceSettings
        [HttpPost]
        public IActionResult SetDeviceSettings([FromBody] SettingDto request)
        {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString();
                var role = _baseData.GetUserRole(token);
                var userId = _baseData.GetUserId(token);
                var data = _userRepository.UserRole(userId, role);
                if (!data.Success) return BadRequest(new { data.ErrorCode, data.Message });
                var userRole = data.Data;
                var result = _deviceRepository.Insert(request,userRole.Id);
                if (result.Success)
                {
                    //save log
                    LogDto log = new()
                    {
                        DateTime = DateTime.Now,
                        FkDeviceId = result.Data,
                        FkUserRoleId = userRole.Id,
                        Type = LogType.SetSettings,
                    };
                    var log_result = _logRepository.Insert(log);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SetDeviceSettings Failed by input: {request}", request);
                return BadRequest(new {ErrorCode.INTERNAL_ERROR, ex.Message});
            }
        }
        #endregion

        #region UpdateSettings
        [HttpPut]
        public IActionResult UpdateSettings([FromBody] SettingDto request)
        {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString();
                var role = _baseData.GetUserRole(token);
                var userId = _baseData.GetUserId(token);
                var data = _userRepository.UserRole(userId, role);
                if (!data.Success) return BadRequest(new { data.ErrorCode, data.Message });
                var userRole = data.Data;
                var result = _deviceRepository.Update(request,userRole.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateSettings Failed by input: {request}", request);
                return BadRequest(new {ErrorCode.INTERNAL_ERROR, ex.Message});
            }
        }
        #endregion

        #region GetSettings
        [HttpPost]
        public IActionResult GetSettings([FromBody] string serial)
        {
            try
            {
                var result = _deviceRepository.GetSettings(serial);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetSettings Failed by input: {request}", serial);
                return BadRequest(new {ErrorCode.INTERNAL_ERROR, ex.Message});
            }
        }
        #endregion

        #region FOTA
        [HttpPost]
        public async Task<IActionResult> FOTA([FromForm] UpdateFOTARequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Settings))
                    return BadRequest(Result.Fail(ErrorCode.NO_CONTENT, "خطای تنظیمات!"));
                if (request.File == null || request.File.Length == 0)
                    return BadRequest(Result.Fail(ErrorCode.NO_FILE_UPLOADED, "هیچ فایلی آپلود نشده است!"));

                var token = Request.Headers[HeaderNames.Authorization].ToString();
                var role = _baseData.GetUserRole(token);
                var userId = _baseData.GetUserId(token);
                var data = _userRepository.UserRole(userId, role);
                if (!data.Success) return BadRequest(new { data.ErrorCode, data.Message });
                var userRole = data.Data;


                var result =await _fotaRepository.InsertAsync(request,userRole.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Post FOTA Failed by input: {request}", request);
                return BadRequest(new { ErrorCode.INTERNAL_ERROR, ex.Message });
            }
        }
        #endregion

        #region DeactivatePrevSameFiles
        [HttpPost]
        public async Task<IActionResult> DeactivatePrevSameFiles([FromForm] UpdateFOTARequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Settings))
                    return BadRequest(Result.Fail(ErrorCode.NO_CONTENT, "خطای تنظیمات!"));
                if (request.File == null || request.File.Length == 0)
                    return BadRequest(Result.Fail(ErrorCode.NO_FILE_UPLOADED, "هیچ فایلی آپلود نشده است!"));

                var token = Request.Headers[HeaderNames.Authorization].ToString();
                var role = _baseData.GetUserRole(token);
                var userId = _baseData.GetUserId(token);
                var data = _userRepository.UserRole(userId, role);
                if (!data.Success) return BadRequest(new { data.ErrorCode, data.Message });
                var userRole = data.Data;


                var result =await _fotaRepository.InsertAndDeactiveSameFiles(request,userRole.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeactivatePrevSameFiles Failed by input: {request}", request);
                return BadRequest(new { ErrorCode.INTERNAL_ERROR, ex.Message });
            }
        }
        #endregion

        #region UploadSoftwareFile
        [HttpPost]
        public async Task<IActionResult> UploadSoftwareFile([FromForm] UploadSoftwareVersion version)
        {
            try
            {
                if (version.Condition is null)
                    return BadRequest(Result.Fail(ErrorCode.NO_CONTENT, "خطای تنظیمات!"));
                if (version.File == null || version.File.Length == 0)
                    return BadRequest(Result.Fail(ErrorCode.NO_FILE_UPLOADED, "هیچ فایلی آپلود نشده است!"));

                var token = Request.Headers[HeaderNames.Authorization].ToString();
                var role = _baseData.GetUserRole(token);
                var userId = _baseData.GetUserId(token);
                var data = _userRepository.UserRole(userId, role);
                if (!data.Success) return BadRequest(new { data.ErrorCode, data.Message });
                var userRole = data.Data;

                var result =await _softwareVersionRepository.Insert(version,userRole.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UploadSoftwareFile Failed by input: {request}", version);
                return BadRequest(new { ErrorCode.INTERNAL_ERROR, ex.Message });
            }
        }
        #endregion

        #region SoftwareFile 
        [HttpPost]
        public IActionResult SoftwareFile([FromBody] int request)
        {
            try
            {
                var path = _softwareVersionRepository.File(request);
                if (string.IsNullOrEmpty(path))
                {
                    return BadRequest(ErrorCode.NO_CONTENT);
                }
                else
                {
                    if (!System.IO.File.Exists(path))
                        return NotFound();
                    string fileName = path.ToString().Split("\\").Last();

                    var file = PhysicalFile(path, "application/octet-stream", fileName);
                    var stream = new FileStream(
                        path,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read
                    );

                    return File(
                        stream,
                        file.ContentType,
                        file.FileName
                    );

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get SoftwareFile Failed by input: {request}", request);
                return BadRequest(new { ErrorCode.INTERNAL_ERROR, ex.Message });
            }
        }
        #endregion

    }
}
