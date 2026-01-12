using Azure.Core;
using GW.Application.Repository;
using GW.Application.Sevices;
using GW.Core.Models.Dto;
using GW.Core.Models.Enum;
using GW.Core.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace GW.SupervisorPanelAPI.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Development")]
    public class FOTAController : ControllerBase
    {
        private readonly ILogger<FOTAController> _logger;
        private readonly IFOTARepository _fotaRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBaseData _baseData;
        private readonly ILogRepository _logRepository;

        #region ctor
        public FOTAController(IFOTARepository fOTARepository
            ,IBaseData baseData
            ,ILogRepository logRepository,
            IUserRepository userRepository
            ,ILogger<FOTAController> logger)
        {
            _baseData = baseData;
            _fotaRepository = fOTARepository;
            _logger = logger;
            _logRepository = logRepository;
            _userRepository = userRepository;
        }
        #endregion

        #region All
        [HttpGet]
        public IActionResult All()
        {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString();
                var role = _baseData.GetUserRole(token);
                var userId = _baseData.GetUserId(token);
                var userDto = _userRepository.User(userId);
                if (userDto is null) return Ok(Result.Fail(ErrorCode.NOT_FOUND, "کاربر غیرمجاز!"));
                var response = _fotaRepository.All(userDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get AllFOTAs Failed");
                return BadRequest(new { ErrorCode.INTERNAL_ERROR, ex.Message });
            }
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FOTADto request)
        {
            Result result = new();
            UserRoleDto userRole = new();
            try
            {
                if (request is null)
                    return BadRequest(Result.Fail(ErrorCode.NO_CONTENT, "خطای تنظیمات!"));

                var token = Request.Headers[HeaderNames.Authorization].ToString();
                var role = _baseData.GetUserRole(token);
                var userId = _baseData.GetUserId(token);
                //userRole = _userRepository.User(userId, role);
                //if (userRole is null) return Ok(Result.Fail(ErrorCode.NOT_FOUND, "کاربر غیرمجاز!"));

                result = await _fotaRepository.InsertAsync(request, userId);
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
        [HttpPut]
        public async Task<IActionResult> DeactivatePrevSameFiles([FromBody] FOTADto request)
        {
            Result result = new();
            UserRoleDto userRole = new();
            try
            {
                if (request is null)
                    return BadRequest(Result.Fail(ErrorCode.NO_CONTENT, "خطای تنظیمات!"));
                //if (request.File == null || request.File.Length == 0)
                //    return BadRequest(Result.Fail(ErrorCode.NO_FILE_UPLOADED, "هیچ فایلی آپلود نشده است!"));

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
    }
}
