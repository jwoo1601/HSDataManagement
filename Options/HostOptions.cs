using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Options
{
    public class HostOptions
    {
        public static readonly string Name = "Host";

        public string Address { get; set; }
        public int Port { get; set; }
        public string[] AllowedSchemes { get; set; }
        public string[] ValidAddresses
            => AllowedSchemes.Select(scheme => $"{scheme}://{Address}").ToArray();
    }
}
