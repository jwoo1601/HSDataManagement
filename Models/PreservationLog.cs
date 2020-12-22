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
    public class PreservationLog
    {
        public int ID { get; set; }
        [Required]
        public MealType MealType { get; set; }
        [Required]
        public MealCategory MealCategory { get; set; }
        [Required]
        public DateTime DateIn { get; set; }
        [Required]
        public DateTime DateOut { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }


        public virtual DailyMenu AssignedMenu { get; set; }
        public virtual EmployeeRole Manager { get; set; }


        public class HSMJsonConverter : JsonConverter<PreservationLog>
        {
            public override PreservationLog ReadJson(JsonReader reader, Type objectType, PreservationLog existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var preservation = new PreservationLog();
                serializer.Populate(reader, preservation);

                return preservation;
            }

            public override void WriteJson(JsonWriter writer, PreservationLog value, JsonSerializer serializer)
            {
                var mappedOperation = new
                {
                    value.ID,
                    value.MealType,
                    value.MealCategory,
                    value.DateIn,
                    value.DateOut,
                    value.CreatedAt,
                    value.LastUpdatedAt,
                    Manager = value.Manager.DisplayName
                };

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, mappedOperation);
            }
        }
    }
}
