using System.Security.Cryptography;

namespace KioskApp.Server.Core.Tokens
{
    public class RandomTokens
    {
        public static string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }


    }
}
