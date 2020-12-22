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
    public class OperationLog
    {
        public int ID { get; set; }
        [Required]
        public MealType MealType { get; set; }
        [Required]
        public int NumCustomersServed { get; set; }
        [Required]
        public int NumEmployeesServed { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }


        public virtual DailyMenu DailyMenu { get; set; }


        public class HSMJsonConverter : JsonConverter<OperationLog>
        {
            public override OperationLog ReadJson(JsonReader reader, Type objectType, OperationLog existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var operation = new OperationLog();
                serializer.Populate(reader, operation);

                return operation;
            }

            public override void WriteJson(JsonWriter writer, OperationLog value, JsonSerializer serializer)
            {
                var mappedOperation = new
                {
                    value.ID,
                    value.MealType,
                    value.NumCustomersServed,
                    value.NumEmployeesServed,
                    value.CreatedAt,
                    value.LastUpdatedAt
                };

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, mappedOperation);
            }
        }
    }
}
