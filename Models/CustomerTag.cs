using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    public class CustomerTag
    {
        public int ID { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [JsonIgnore]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }


        [JsonIgnore]
        public virtual ICollection<CustomerTagAssignment> CustomerAssignments { get; set; }
    }
}
