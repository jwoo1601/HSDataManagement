using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Options
{
    public class AccountOptions
    {
        public static readonly string Name = "Accounts";

        public bool AllowRememberLogin { get; set; } = true;
        public TimeSpan RememberLoginDuration { get; set; } = TimeSpan.FromDays(30);
    }
}
