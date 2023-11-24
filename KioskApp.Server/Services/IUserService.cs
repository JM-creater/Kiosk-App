using KioskApp.Model.Dto;
using KioskApp.Model.Entities;
using KioskApp.Server.Core.Enum;

namespace KioskApp.Server.Services
{
    public interface IUserService
    {
        public Task<User> Register(RegisterUserDto dto);
        public Task<(User user, UserRole role)> Login(UserLoginDto dto);
        public Task<List<User>> GetAllUsers();
        public Task<User> Verify(string token);
        public Task<User> ForgotPassword(string email);
        public Task<User> ResetPassword(ResetPasswordDto dto);
    }
}
