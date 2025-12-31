using GW.Application.Repository;
using GW.Application.Sevices;
using GW.Core.Models.Dto;
using GW.Core.Models.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GW.SupervisorPanelAPI.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IBaseData _baseData;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository,IBaseData baseData
            ,ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _baseData = baseData;
            _logger = logger;
        }

        #region Login
        [HttpPost]
        public IActionResult Login([FromBody]LoginInfo request)
        {
            try
            {
                var response = _userRepository.Login(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login Failed ");
                return BadRequest(new {ErrorCode.INTERNAL_ERROR, ex.Message});
            }
        }
        #endregion

        #region Token
        [HttpPost]
        public IActionResult Token([FromBody]LoginInfo request)
        {
            try
            {
                var response = _userRepository.Token(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Token Failed ");
                return BadRequest(new {ErrorCode.INTERNAL_ERROR, ex.Message});
            }
        }
        #endregion

        #region Hash
        [HttpPost]
        public IActionResult Hash([FromBody]string pass)
        {
            try
            {
                var response = _baseData.HashMaker(pass);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new {ErrorCode.INTERNAL_ERROR, ex.Message});
            }
        }
        #endregion

    }
}
