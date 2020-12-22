using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.ViewModels
{
    public class RegisterViewModel
    {
        public string DefaultUsername { get; set; }
        public string DefaultName { get; set; }
        public string DefaultEmail { get; private set; }
        public string DefaultEmailDomain { get; private set; }
        public string DefaultSecurityCode { get; set; }

        public RegisterViewModel SetDefaultEmailAddress(string emailAddress)
        {
            var emailComponents = emailAddress?.Split('@');
            if (emailComponents != null && emailComponents.Length >= 2)
            {
                DefaultEmail = emailComponents?[0];
                DefaultEmailDomain = emailComponents?[1];
            }

            return this;
        }
    }
}
