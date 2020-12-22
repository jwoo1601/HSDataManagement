using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.ViewModels
{
    public class SuccessViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public bool EnableAutoRedirection { get; set; } = false;
        public string AutoRedirectionUrl { get; set; } = "/";
        public TimeSpan AutoRedirectionDelay { get; set; } = TimeSpan.FromSeconds(3);
    }
}
