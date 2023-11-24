using KioskApp.Model.Dto;
using KioskApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace KioskApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OtpController : ControllerBase
    {
        private readonly OtpService _otpService;
        public OtpController(OtpService otpService)
        {
            _otpService = otpService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendOtp([FromBody] OtpDto request)
        {
            var otp = _otpService.GenerateOtp(request.UserId);
            await _otpService.SendOtpAsync(request.PhoneNumber, otp);
            return Ok();
        }

        [HttpPost("validate")]
        public IActionResult ValidateOtp([FromBody] OtpValidationDto request)
        {
            var isValid = _otpService.ValidateOtp(request.UserId, request.Otp);
            return Ok(isValid);
        }
    }
}
