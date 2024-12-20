using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApp.Core.Utilities.Security
{
    public interface IPasswordSecurity
    {
        static abstract string HashPassword(string password, byte[] salt);
        static abstract byte[] GenerateSalt();

        static abstract bool VerifyPassword(string password, string storedHash, byte[] storedSalt);
    }
}
