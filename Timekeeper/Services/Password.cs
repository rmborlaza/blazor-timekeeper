using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Timekeeper.Services
{
    public class Password
    {
        public static string CalculateHash(string password)
        {
            byte[] salt = new byte[128 / 8];

            string hashed = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password,
                    salt,
                    KeyDerivationPrf.HMACSHA256,
                    10000,
                    256 / 8));

            return hashed;
        }

        public static bool ValidatePassword(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }
            return true;
        }
    }
}
