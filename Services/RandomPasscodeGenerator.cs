using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HyosungManagement.Services
{
    public class RandomPasscodeGenerator : IRandomCodeGenerator
    {
        public static readonly int DefaultNumBytes = 16;

        public string GenerateRandomPasscode(PasscodeGenerationHints hints)
        {
            int numBytes = hints.NumBytes < 1 ? DefaultNumBytes : hints.NumBytes;

            byte[] rawBytes;
            if (hints.IsUnique)
            {
                rawBytes = Guid.NewGuid().ToByteArray();
            }
            else
            {
                using (var crypto = new RNGCryptoServiceProvider())
                {
                    rawBytes = new byte[numBytes];
                    crypto.GetBytes(rawBytes);
                }
            }

            switch (hints.TextFormat)
            {
                case PasscodeTextFormat.Base64:
                    return WebEncoders.Base64UrlEncode(rawBytes);
                case PasscodeTextFormat.Guid:
                    return new Guid(rawBytes).ToString("N");
            }

            return null;
        }
    }
}
