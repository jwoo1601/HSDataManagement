using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.InputModels
{
    public class CustomerSetOptionsInputModel
    {
        public bool? Visible { get; set; }
        public bool? Discharged { get; set; }
        public DateTime? DischargeDate { get; set; }
    }
}
