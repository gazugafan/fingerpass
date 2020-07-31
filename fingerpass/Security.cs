using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace gazugafan.fingerpass
{
	class Security
	{
        /// <summary>
        /// Generates a cryptographically secure random string
        /// </summary>
        /// <param name="length"></param>
        /// <param name="allowableChars"></param>
        /// <returns></returns>
        public static string GenerateRandomString(int length = 32, string allowableChars = null)
        {
            if (string.IsNullOrEmpty(allowableChars))
                allowableChars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

            // Generate random data
            var rnd = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(rnd);

            // Generate the output string
            var allowable = allowableChars.ToCharArray();
            var l = allowable.Length;
            var chars = new char[length];
            for (var i = 0; i < length; i++)
                chars[i] = allowable[rnd[i] % l];

            return new string(chars);
        }

        /// <summary>
        /// Gets the currently logged in user's SID. Useful to check that their fingerprint matches
        /// </summary>
        public static string GetSID()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().User.ToString();
        }
    }
}
