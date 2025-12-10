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
    [Authorize("Development")]
    public class DevelopmentController : ControllerBase
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IBaseData _baseData;
        private readonly ISettingsService _settingsService;

        public DevelopmentController(IDeviceRepository deviceRepository,IBaseData baseData,ISettingsService settingsService)
        {
            _deviceRepository = deviceRepository;
            _baseData = baseData;
            _settingsService = settingsService;
        }

        #region BaseInfo
        [HttpGet]
        public IActionResult BaseInfo()
        {
            try
            {
                var result = _settingsService.Info();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ErrorCode.INTERNAL_ERROR, ex.Message });
            }
        }
        #endregion

        //todo get softversions

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
