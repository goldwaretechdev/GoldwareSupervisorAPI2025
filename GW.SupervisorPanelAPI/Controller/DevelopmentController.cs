using GW.Application.Repository;
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

        public DevelopmentController(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

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
