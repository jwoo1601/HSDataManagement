using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.ViewModels
{
    public class LoginViewModel
    {
        public string RedirectUrl { get; set; }
        public string DefaultUsername { get; set; }
        public bool DefaultRememberLogin { get; set; }
    }
}
