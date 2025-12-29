using GW.Application.Repository;
using GW.Application.Sevices;
using GW.Core.Models.Dto;
using GW.Core.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GW.SupervisorPanelAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceManagmentController : ControllerBase
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IFOTARepository _fotaRepository;
        private readonly IBaseData _baseData;
        private readonly ISettingsService _settingsService;
        private readonly IMemoryCache _cache;

        public DeviceManagmentController(IDeviceRepository deviceRepository, IBaseData baseData
            , ISettingsService settingsService
            , IFOTARepository fOTARepository
            , IMemoryCache memoryCache)
        {
            _deviceRepository = deviceRepository;
            _baseData = baseData;
            _settingsService = settingsService;
            _fotaRepository = fOTARepository;
            _cache = memoryCache;
        }

        #region Check
        [HttpPost("[action]")]
        public async Task<IActionResult> Check([FromBody] string request)
        {
            try
            {
                var setting = _baseData.ConvertStringToSettings(request);
                var fota = _fotaRepository.Check(setting);
                if (fota is null) return Ok(Result<DeviceCheckDto>
                    .Ok(new DeviceCheckDto { AccessCode = ErrorCode.NO_CONTENT, Type = ErrorCode.NO_CONTENT }));

                //change url id
                Constants.FOTA_URL_IDENTITY = Guid.NewGuid();
                // access for minutes
                _cache.Set(Constants.FOTA_URL_IDENTITY.ToString(), fota.Path, TimeSpan.FromMinutes(Constants.FOTA_TIMER));

                var type = string.Empty;
                if(fota.FkESPId.HasValue) type=Constants.FOTA_ESP_FILE_CONTENT;
                if(fota.FkHoltekId.HasValue) type=Constants.FOTA_HOLTECK_FILE_CONTENT;
                if(fota.FkSTMId.HasValue) type=Constants.FOTA_STM_FILE_CONTENT;
                var result = new DeviceCheckDto
                {
                    AccessCode = Constants.FOTA_URL_IDENTITY.ToString(),
                    Type = type,
                };
                return Ok(Result<DeviceCheckDto>.Ok(result));
            }
            catch (Exception ex)
            {
                return BadRequest(Result<string>.Fail(ErrorCode.INTERNAL_ERROR, ex.Message));
            }
        }
        #endregion

        #region FOTAFile
        [HttpGet("[action]/{route}")]
        public IActionResult FOTAFile(string route)
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

        #region Verify
        [HttpPost("[action]")]
        public IActionResult Verify([FromBody]FOTAVerify request)
        {
            try
            {
                return Ok(Result.Ok());
                //  Check access
                //var path = _cache.Get(route);
                //if (string.IsNullOrEmpty((string?)path))
                //{
                //    return BadRequest(ErrorCode.NO_CONTENT);
                //}
                //else
                //{
                //    if (!System.IO.File.Exists((string?)path))
                //        return NotFound();
                //    string fileName = path.ToString().Split("\\").Last();
                //    return PhysicalFile((string)path, "application/octet-stream", fileName);
                //}
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Fail(ErrorCode.INTERNAL_ERROR,""));
            }
        }
        #endregion

    }
}
