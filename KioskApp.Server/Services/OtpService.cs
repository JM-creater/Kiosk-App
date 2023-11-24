using System.Collections.Concurrent;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace KioskApp.Server.Services
{
    public class OtpService : IOtpService
    {
        private readonly ConcurrentDictionary<string, string> _otpStore = new ConcurrentDictionary<string, string>();

        public string GenerateOtp(string userId)
        {
            var otp = new Random().Next(1000, 9999).ToString();
            _otpStore[userId] = otp;
            return otp;
        }

        public async Task SendOtpAsync(string phoneNumber, string otp)
        {
            string accountSid = Environment.GetEnvironmentVariable("ACca5bcafa5e6297e972600f32361a3590");
            string authToken = Environment.GetEnvironmentVariable("10e687e3e686659e7290df89e136fdf2");

            TwilioClient.Init(accountSid, authToken);

            await MessageResource.CreateAsync(
                body: $"Your OTP is: {otp}",
                from: new Twilio.Types.PhoneNumber(Environment.GetEnvironmentVariable("+63 919 943 1060")),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );
        }

        public bool ValidateOtp(string userId, string otp)
        {
            if (_otpStore.TryGetValue(userId, out var validOtp))
            {
                return otp == validOtp;
            }

            return false;
        }
    }
}
