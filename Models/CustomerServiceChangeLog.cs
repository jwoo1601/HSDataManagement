using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    public class CustomerServiceChangeLog
    {
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public int ServiceID { get; set; }
        [Required]
        public MealCategory Category { get; set; }
        [Required]
        public DateTime EffectiveDate { get; set; }

        [JsonIgnore]
        public Customer Customer { get; set; }
        [JsonIgnore]
        public Service Service { get; set; }
    }
}
