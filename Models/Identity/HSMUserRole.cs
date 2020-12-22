using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models.Identity
{
    public class HSMUserRole : IdentityUserRole<string>
    {
        public virtual HSMUser User { get; set; }
        public virtual HSMRole Role { get; set; }
    }
}
