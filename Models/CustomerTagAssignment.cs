using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    public class CustomerTagAssignment
    {
        public int CustomerID { get; set; }
        public int CustomerTagID { get; set; }

        [JsonIgnore]
        public virtual Customer Customer { get; set; }
        [JsonIgnore]
        public virtual CustomerTag Tag { get; set; }
    }
}
