namespace KioskApp.Server.Services
{
    public interface IOtpService
    {
        public Task SendOtpAsync(string phoneNumber, string otp);
    }
}
