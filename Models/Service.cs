using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    [JsonConverter(typeof(HSMJsonConverter))]
    public class Service
    {
        public int ID { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Note { get; set; }
        public TimeSpan? Duration { get; set; }
        public int? GroupID { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }


        public virtual ServiceGroup Group { get; set; }
        public virtual ICollection<CustomerServiceAssignment> CustomerAssignments { get; set; }


        public class HSMJsonConverter : JsonConverter<Service>
        {
            public override Service ReadJson(JsonReader reader, Type objectType, Service existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var service = new Service();
                serializer.Populate(reader, service);

                return service;
            }

            public override void WriteJson(JsonWriter writer, Service value, JsonSerializer serializer)
            {
                var mappedService = new
                {
                    value.ID,
                    value.Name,
                    value.Note,
                    value.Duration,
                    value.Group,
                    value.CreatedAt,
                    value.LastUpdatedAt
                };

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, mappedService);
            }
        }
    }
}
