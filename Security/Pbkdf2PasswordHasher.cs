// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Pokefans.Security.Cryptography;

namespace Pokefans.Security
{
    public class Pbkdf2PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            byte[] salt = new byte[32];
            Random r = new Random();

            r.NextBytes(salt);

            Pbkdf2 pbkdf2 = new Pbkdf2(new HMACSHA256(), System.Text.Encoding.UTF8.GetBytes(password), salt);
            return Convert.ToBase64String(pbkdf2.GetBytes(32)) + ":" + Convert.ToBase64String(salt);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            string[] oldpw = hashedPassword.Split(":".ToCharArray());

            byte[] salt = Convert.FromBase64String(oldpw[1]);

            Pbkdf2 pbkdf2 = new Pbkdf2(new HMACSHA256(), System.Text.Encoding.UTF8.GetBytes(providedPassword), salt);
            
            if (Convert.ToBase64String(pbkdf2.GetBytes(32)) == oldpw[0])
                return PasswordVerificationResult.Success;

            // TODO: Implement converting of old phpBB passwords
            // Possible Implementation is here https://www.phpbb.com/community/viewtopic.php?f=71&t=1771165, but cannot be used due to licensing issues.

            //if (cPhpBB.phpbbCheckHash(providedPassword, hashedPassword))
            //    return PasswordVerificationResult.SuccessRehashNeeded;

            return PasswordVerificationResult.Failed;
        }
    }
}
