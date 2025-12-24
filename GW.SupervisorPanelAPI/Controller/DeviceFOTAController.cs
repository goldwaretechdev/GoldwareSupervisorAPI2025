using GW.Application.Repository;
using GW.Application.Sevices;
using GW.Core.Models.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;

namespace GW.SupervisorPanelAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceFOTAController : ControllerBase
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IFOTARepository _fotaRepository;
        private readonly IBaseData _baseData;
        private readonly ISettingsService _settingsService;
        private readonly IMemoryCache _cache;

        public DeviceFOTAController(IDeviceRepository deviceRepository, IBaseData baseData
            , ISettingsService settingsService
            , IFOTARepository fOTARepository
            ,IMemoryCache memoryCache)
        {
            _deviceRepository = deviceRepository;
            _baseData = baseData;
            _settingsService = settingsService;
            _fotaRepository = fOTARepository;
            _cache = memoryCache;
        }

        #region CheckFOTA
        [HttpPost("[action]")]
        public async Task<IActionResult> CheckFOTA([FromBody] string request)
        {
            try
            {
                var setting = _baseData.ConvertStringToSettings(request);
                var path = _fotaRepository.Check(setting);
                if (string.IsNullOrEmpty(path)) return Ok(Result<string>.Ok(Constants.NO_DATA));

                //change url id
                Constants.FOTA_URL_IDENTITY = Guid.NewGuid();

                // access for 5 minutes
                _cache.Set(Constants.FOTA_URL_IDENTITY.ToString(), path, TimeSpan.FromMinutes(Constants.FOTA_TIMER));
                return Ok(Result<string>.Ok(Constants.FOTA_URL_IDENTITY.ToString()));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<string>.Fail(ErrorCode.INTERNAL_ERROR, ex.Message));
            }
        }
        #endregion

        #region GetFOTA
        [HttpPost("[action]/{route}")]
        public IActionResult FOTA(string route)
        {
            try
            {
                //  Check access
                var path = _cache.Get(route);
                if (string.IsNullOrEmpty((string?)path))
                {
                    return BadRequest(ErrorCode.NO_CONTENT);
                }
                else
                {
                    if (!System.IO.File.Exists((string?)path))
                        return NotFound();
                    string fileName = path.ToString().Split("\\").Last();
                    return PhysicalFile((string)path, "application/octet-stream", fileName);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorCode.INTERNAL_ERROR);
            }
        }
        #endregion

    }
}
