using Azure.Core;
using GW.Application.Repository;
using GW.Application.Sevices;
using GW.Core.Models;
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
        private readonly ILogRepository _logRepository;
        private readonly IBaseData _baseData;
        private readonly IMemoryCache _cache;
        private readonly ILogger<DeviceManagmentController> _logger;

        #region ctor
        public DeviceManagmentController(IDeviceRepository deviceRepository
            , IBaseData baseData
            , IFOTARepository fOTARepository
            ,ILogRepository logRepository
            , IMemoryCache memoryCache
            ,ILogger<DeviceManagmentController> logger)
        {
            _deviceRepository = deviceRepository;
            _baseData = baseData;
            _fotaRepository = fOTARepository;
            _cache = memoryCache;
            _logRepository= logRepository;
            _logger = logger;
        }
        #endregion

        #region Check
        [HttpPost("[action]")]
        public async Task<IActionResult> Check([FromBody] CheckRequest request)
        {
            try
            {
                var setting = _baseData.ConvertStringToSettings(request.SetSetting);
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
                var device = _deviceRepository.GetDeviceByUniqueId(request.UniqueId);
                if (!device.Success) return Ok(device);
                LogDto log = new()
                {
                    DateTime = DateTime.Now,
                    FkDeviceId = device.Data.Id,
                    Type = Core.Models.Enum.LogType.FOTA_Update_Requested,
                    Desc = "Type = " + type + ", FOTA_Id = " + fota.Id
                };
                var log_result = _logRepository.Insert(log);
                return Ok(Result<DeviceCheckDto>.Ok(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Check Failed by input: {request}", request);
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
                _logger.LogError(ex, "Get FOTAFile Failed");
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
                var device = _deviceRepository.GetDeviceByUniqueId(request.UniqueId);
                if (!device.Success) return Ok(device);
                LogDto log = new()
                {
                    DateTime = DateTime.Now,
                    FkDeviceId = device.Data.Id,
                    Type = Core.Models.Enum.LogType.FOTA_Update_Done,
                    Desc = "ErrorCode = " + request.ErrorCode 
                    + " , Desc= " + request.Message
                    + " , Type= " + request.Type
                };
                var result = _logRepository.Insert(log);
                return Ok(Result.Ok());
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Verify by input: {request}", request);
                return BadRequest(Result.Fail(ErrorCode.INTERNAL_ERROR,""));
            }
        }
        #endregion

    }
}
