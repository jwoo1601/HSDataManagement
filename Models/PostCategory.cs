using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    public class PostCategory
    {
        public int ID { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool ShowBoard { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }


        [JsonIgnore]
        public virtual ICollection<Post> Posts { get; set; }
    }
}
