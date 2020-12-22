using HyosungManagement.Models.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    public enum SecurityCodeType
    {
        Normal,
        Transient,
        Persistent
    }

    public class SecurityCode
    {
        public int ID { get; set; }
        [Required]
        public SecurityCodeType CodeType { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public bool IsValid { get; set; }
        [Required]
        public DateTime GeneratedAt { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ExpiresAt { get; set; }

        [NotMapped]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsExpired
        {
            get
            {
                if (ExpiresAt.HasValue)
                {
                    return DateTime.Now > ExpiresAt.Value;
                }

                return null;
            }
        }


        public virtual ICollection<HSMUser> Users { get; set; }
    }
}
