using Azure.Core;
using GW.Application.Repository;
using GW.Application.Sevices;
using GW.Core.Models.Dto;
using GW.Core.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GW.SupervisorPanelAPI.Controller
{
    [Route("api/[controller]/[action]" )]
    [ApiController]
    [Authorize(Roles ="Development")]
    public class SettingsController : ControllerBase
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IBaseData _baseData;
        private readonly ISettingsService _settingsService;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ISoftwareVersionRepository _softwareVersionRepository;

        public SettingsController(IDeviceRepository deviceRepository,IBaseData baseData
            ,ISettingsService settingsService,IOwnerRepository ownerRepository
            ,ISoftwareVersionRepository softwareVersion)
        {
            _deviceRepository = deviceRepository;
            _baseData = baseData;
            _settingsService = settingsService;
            _ownerRepository = ownerRepository;
            _softwareVersionRepository = softwareVersion;
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
                var result = _deviceRepository.Insert(request);
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

    }
}
