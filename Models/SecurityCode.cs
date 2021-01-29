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

    [JsonConverter(typeof(HSMJsonConverter))]
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


        public class HSMJsonConverter : JsonConverter<SecurityCode>
        {
            public override SecurityCode ReadJson(JsonReader reader, Type objectType, SecurityCode existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var code = new SecurityCode();
                serializer.Populate(reader, code);

                return code;
            }

            public override void WriteJson(JsonWriter writer, SecurityCode value, JsonSerializer serializer)
            {
                var mappedCode = new
                {
                    value.ID,
                    value.CodeType,
                    value.Value,
                    value.IsValid,
                    value.GeneratedAt,
                    value.ExpiresAt
                };

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, mappedCode);
            }
        }
    }
}
