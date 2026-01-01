using GW.Application.Repository;
using GW.Application.Sevices;
using GW.Core.Models.Dto;
using GW.Core.Models.Enum;
using GW.Core.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace GW.SupervisorPanelAPI.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Development")]
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
        public SettingsController(IDeviceRepository deviceRepository, IBaseData baseData
            , IOwnerRepository ownerRepository
            , ISoftwareVersionRepository softwareVersion, IUserRepository userRepository
            , IFOTARepository fOTARepository
            , ILogRepository logRepository,
            ILogger<SettingsController> logger)
        {
            _deviceRepository = deviceRepository;
            _userRepository = userRepository;
            _baseData = baseData;
            _ownerRepository = ownerRepository;
            _softwareVersionRepository = softwareVersion;
            _fotaRepository = fOTARepository;
            _logRepository = logRepository;
            _logger = logger;
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
        public IActionResult SoftwareVersions([FromBody] RequestVersions request)
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
            Result<int> result = new();
            UserRoleDto userRole = new();
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString();
                var role = _baseData.GetUserRole(token);
                var userId = _baseData.GetUserId(token);
                userRole = _userRepository.UserRole(userId, role);
                if (userRole is null) return Ok(Result.Fail(ErrorCode.NOT_FOUND, "کاربر غیرمجاز!"));
                result = _deviceRepository.Insert(request, userRole.Id);
               
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SetDeviceSettings Failed by input: {request}", request);
                return BadRequest(new { ErrorCode.INTERNAL_ERROR, ex.Message });
            }
            finally
            {
                if (result?.Success == true)
                {
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            LogDto log = new()
                            {
                                DateTime = DateTime.Now,
                                FkDeviceId = result.Data,
                                FkUserRoleId = userRole.Id,
                                Type = LogType.SetSettings,
                                Desc = $"دستگاه با سریال {request.SerialNumber} ثبت شد"
                            };
                            var log_result = _logRepository.Insert(log);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Background log failed");
                        }
                    });
                }
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
                var userRole = _userRepository.UserRole(userId, role);
                if (userRole is null) return Ok(Result.Fail(ErrorCode.NOT_FOUND, "کاربر غیرمجاز!"));
                var result = _deviceRepository.Update(request, userRole.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateSettings Failed by input: {request}", request);
                return BadRequest(new { ErrorCode.INTERNAL_ERROR, ex.Message });
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
                return BadRequest(new { ErrorCode.INTERNAL_ERROR, ex.Message });
            }
        }
        #endregion

        #region FOTA
        [HttpPost]
        public async Task<IActionResult> FOTA([FromForm] UpdateFOTARequest request)
        {
            Result result = new();
            UserRoleDto userRole = new();
            try
            {
                if (string.IsNullOrEmpty(request.Settings))
                    return BadRequest(Result.Fail(ErrorCode.NO_CONTENT, "خطای تنظیمات!"));
                if (request.File == null || request.File.Length == 0)
                    return BadRequest(Result.Fail(ErrorCode.NO_FILE_UPLOADED, "هیچ فایلی آپلود نشده است!"));

                var token = Request.Headers[HeaderNames.Authorization].ToString();
                var role = _baseData.GetUserRole(token);
                var userId = _baseData.GetUserId(token);
                userRole = _userRepository.UserRole(userId, role);
                if (userRole is null) return Ok(Result.Fail(ErrorCode.NOT_FOUND, "کاربر غیرمجاز!"));

                result = await _fotaRepository.InsertAsync(request, userRole.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Post FOTA Failed by input: {request}", request);
                return BadRequest(new { ErrorCode.INTERNAL_ERROR, ex.Message });
            }
            finally
            {
                if (result?.Success == true)
                {
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            LogDto logDto = new()
                            {
                                DateTime = DateTime.Now,
                                Desc = $"آپلود فایل FOTA انجام شد.",
                                Type = LogType.Update_FOTA_File,
                                FkUserRoleId = userRole.Id,
                            };
                            var log = _logRepository.Insert(logDto);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Background log failed");
                        }
                    });
                }
            }
        }
        #endregion

        #region DeactivatePrevSameFiles
        [HttpPost]
        public async Task<IActionResult> DeactivatePrevSameFiles([FromForm] UpdateFOTARequest request)
        {
            Result result = new();
            UserRoleDto userRole = new();
            try
            {
                if (string.IsNullOrEmpty(request.Settings))
                    return BadRequest(Result.Fail(ErrorCode.NO_CONTENT, "خطای تنظیمات!"));
                if (request.File == null || request.File.Length == 0)
                    return BadRequest(Result.Fail(ErrorCode.NO_FILE_UPLOADED, "هیچ فایلی آپلود نشده است!"));

                var token = Request.Headers[HeaderNames.Authorization].ToString();
                var role = _baseData.GetUserRole(token);
                var userId = _baseData.GetUserId(token);
                userRole = _userRepository.UserRole(userId, role);
                if (userRole is null) return Ok(Result.Fail(ErrorCode.NOT_FOUND, "کاربر غیرمجاز!"));

                result = await _fotaRepository.InsertAndDeactiveSameFiles(request, userRole.Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeactivatePrevSameFiles Failed by input: {request}", request);
                return BadRequest(new { ErrorCode.INTERNAL_ERROR, ex.Message });
            }
            finally
            {
                if (result?.Success == true)
                {
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            LogDto logDto = new()
                            {
                                DateTime = DateTime.Now,
                                Desc = $"آپلود فایل FOTA و غیر فعالسازی فایل های مشابه انجام شد.",
                                Type = LogType.Update_FOTA_File,
                                FkUserRoleId = userRole.Id,
                            };
                            var log = _logRepository.Insert(logDto);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Background log failed");
                        }
                    });
                }
            }
        }
        #endregion

        #region UploadSoftwareFile
        [HttpPost]
        public async Task<IActionResult> UploadSoftwareFile([FromForm] UploadSoftwareVersion version)
        {
            Result result = new();
            UserRoleDto userRole = new();
            try
            {
                if (version.Condition is null)
                    return BadRequest(Result.Fail(ErrorCode.NO_CONTENT, "خطای تنظیمات!"));
                if (version.File == null || version.File.Length == 0)
                    return BadRequest(Result.Fail(ErrorCode.NO_FILE_UPLOADED, "هیچ فایلی آپلود نشده است!"));

                var token = Request.Headers[HeaderNames.Authorization].ToString();
                var role = _baseData.GetUserRole(token);
                var userId = _baseData.GetUserId(token);
                userRole = _userRepository.UserRole(userId, role);
                if (userRole is null) return Ok(Result.Fail(ErrorCode.NOT_FOUND, "کاربر غیرمجاز!"));

                result = await _softwareVersionRepository.Insert(version, userRole.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UploadSoftwareFile Failed by input: {request}", version);
                return BadRequest(new { ErrorCode.INTERNAL_ERROR, ex.Message });
            }
            finally
            {
                if (result?.Success == true)
                {
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            LogDto logDto = new()
                            {
                                DateTime = DateTime.Now,
                                Desc = $"آپلود فایل Software version انجام شد.",
                                Type = LogType.Insert_SoftwareVersion_File,
                                FkUserRoleId = userRole.Id,
                            };
                            var log = _logRepository.Insert(logDto);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Background log failed");
                        }
                    });
                }
            }
        }
        #endregion

        #region SoftwareFile 
        [HttpPost]
        public IActionResult SoftwareFile([FromBody] int request)
        {
            Result result = new();
            UserRoleDto userRole = new();
            SoftwareVersionDto soft = new();
            try
            {
                soft = _softwareVersionRepository.File(request);
                if (soft is null) return Ok(Result.Fail(ErrorCode.INVALID_ID, "شناسه نامعتبر"));
                else
                {
                    if (!System.IO.File.Exists(soft.Path))
                        return Ok(Result.Fail(ErrorCode.NOT_FOUND, "فایل پیدا نشد!"));
                    string fileName = soft.Path.ToString().Split("\\").Last();

                    var file = PhysicalFile(soft.Path, "application/octet-stream", fileName);
                    var stream = new FileStream(
                        soft.Path,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read
                    );
                    result = Result.Ok();
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
            finally
            {
                if (result?.Success == true)
                {
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            var token = Request.Headers[HeaderNames.Authorization].ToString();
                            var role = _baseData.GetUserRole(token);
                            var userId = _baseData.GetUserId(token);
                            userRole = _userRepository.UserRole(userId, role);
                            if (userRole is not null)
                            {
                                LogDto logDto = new()
                                {
                                    DateTime = DateTime.Now,
                                    Desc = $"programming {soft.MicroType.ToString()}.",
                                    Type = LogType.Programming,
                                    FkUserRoleId = userRole.Id,
                                };
                                var log = _logRepository.Insert(logDto);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Background log failed");
                        }
                    });
                }

            }
        }
        #endregion

    }
}
