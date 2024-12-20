using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApp.Core.Utilities.Security
{
    public class PasswordSecurity: IPasswordSecurity
    {
        public static string HashPassword(string password, byte[] salt)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                return Convert.ToBase64String(rfc2898.GetBytes(32)); // 32-byte key
            }
        }

        public static byte[] GenerateSalt()
        {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        public static bool VerifyPassword(string password, string storedHash, byte[] storedSalt)
        {
            var hashedPassword = HashPassword(password, storedSalt);
            return hashedPassword == storedHash;
        }
    }
}
