using Azure.Core;
using GW.Application.Repository;
using GW.Application.Sevices;
using GW.Core.Models;
using GW.Core.Models.Dto;
using GW.Core.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace GW.SupervisorPanelAPI.Controller
{
    [Route("api/[controller]/[action]" )]
    [ApiController]
    [Authorize(Roles ="Development")]
    public class SettingsController : ControllerBase
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IFOTARepository _fotaRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBaseData _baseData;
        private readonly ISettingsService _settingsService;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ISoftwareVersionRepository _softwareVersionRepository;

        public SettingsController(IDeviceRepository deviceRepository,IBaseData baseData
            ,ISettingsService settingsService,IOwnerRepository ownerRepository
            ,ISoftwareVersionRepository softwareVersion,IUserRepository userRepository
            ,IFOTARepository fOTARepository)
        {
            _deviceRepository = deviceRepository;
            _userRepository = userRepository;
            _baseData = baseData;
            _settingsService = settingsService;
            _ownerRepository = ownerRepository;
            _softwareVersionRepository = softwareVersion;
            _fotaRepository = fOTARepository;
        }

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
                return Ok(result);
            }
            catch (Exception ex)
            {
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
                var result = _deviceRepository.Update(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
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
                var result =await _fotaRepository.InsertAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ErrorCode.INTERNAL_ERROR, ex.Message });
            }
        }
        #endregion       
    }
}
