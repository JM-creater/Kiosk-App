using KioskApp.Model.Dto;
using KioskApp.Model.Entities;
using KioskApp.Server.Core.Enum;
using KioskApp.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace KioskApp.Server.Controllers
{
    [ApiController, Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService service;
        public UserController(IUserService _service)
        {
            service = _service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto dto)
        {
            var user = await service.Register(dto);

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserLoginDto dto)
        {
            (User user, UserRole role) = await service.Login(dto);

            return new JsonResult(new { user, role = role.ToString() });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var user = await service.GetAllUsers();

            return Ok(user);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyLogin(string token)
        {
            var user = await service.Verify(token);

            return Ok(user);    
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await service.ForgotPassword(email);

            return Ok(user);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var user = await service.ResetPassword(dto);

            return Ok(user);
        }
    }
}
