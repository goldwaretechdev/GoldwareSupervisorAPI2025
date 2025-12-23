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
        [HttpPost]
        public async Task<IActionResult> CheckFOTA([FromBody] string request)
        {
            try
            {
                var setting = _baseData.ConvertStringToSettings(request);
                if (await _fotaRepository.Check(setting)) return Ok(Result<string>.Ok(Constants.NO_DATA));

                //  Check lock
                if (_cache.TryGetValue(Constants.FOTA_URL_IDENTITY.ToString(), out _))
                {
                    return Ok(Result<string>.Ok(ErrorCode.ACTION_LOCKED));
                }
                //change url id
                Constants.FOTA_URL_IDENTITY = Guid.NewGuid();
                // Lock for 5 minutes
                _cache.Set(Constants.FOTA_URL_IDENTITY.ToString(), true, TimeSpan.FromMinutes(5));
                return Ok(Result<string>.Ok(Constants.FOTA_URL_IDENTITY.ToString()));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<string>.Fail(ErrorCode.INTERNAL_ERROR, ex.Message));
            }
        }
        #endregion

        #region GetFOTA
        [HttpPost("/{route}")]
        public IActionResult FOTA(Guid route)
        {
            try
            {

                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }
}
