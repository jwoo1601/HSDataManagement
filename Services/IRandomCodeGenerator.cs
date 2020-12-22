using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Services
{
    public enum PasscodeTextFormat
    {
        Base64,
        Guid
    }

    public class PasscodeGenerationHints
    {
        public bool IsUnique { get; set; } = false;
        public int NumBytes { get; set; }
        public PasscodeTextFormat TextFormat { get; set; } = PasscodeTextFormat.Base64;
    }

    public interface IRandomCodeGenerator
    {
        string GenerateRandomPasscode(PasscodeGenerationHints hints);
    }
}
