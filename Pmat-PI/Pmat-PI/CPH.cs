using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Pmat_PI
{
    public class CPH<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        public string HashPassword(TUser user, string password)
        {
            var sha512 = new SHA512Managed();
            var bytes = UTF8Encoding.UTF8.GetBytes(password);
            var hash = sha512.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword == HashPassword(user, providedPassword)) {
                return PasswordVerificationResult.Success;
            }
            return PasswordVerificationResult.Failed;
        }
    }
}
