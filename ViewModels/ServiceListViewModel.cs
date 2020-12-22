using HyosungManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.ViewModels
{
    public class ServiceListViewModel
    {
        public IEnumerable<Service> Services { get; set; }
    }
}
