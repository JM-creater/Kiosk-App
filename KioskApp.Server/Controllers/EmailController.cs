using KioskApp.Model.Dto;
using KioskApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace KioskApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] EmailDto emailDto)
        {
            try
            {
                await _emailService.SendEmailAsync(emailDto.Email, emailDto.Subject, emailDto.Message);
                return Ok("Email sent successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}
