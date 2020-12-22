using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    public class CustomerServiceAssignment
    {
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public MealType MealType { get; set; }
        [Required]
        public int ServiceID { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Service Service { get; set; }
    }
}
