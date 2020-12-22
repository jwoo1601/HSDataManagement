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
    public class FoodCategory
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }


        public virtual ICollection<Food> Foods { get; set; }


        public class HSMJsonConverter : JsonConverter<FoodCategory>
        {
            public override FoodCategory ReadJson(JsonReader reader, Type objectType, FoodCategory existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var category = new FoodCategory();
                serializer.Populate(reader, category);

                return category;
            }

            public override void WriteJson(JsonWriter writer, FoodCategory value, JsonSerializer serializer)
            {
                var mappedCategory = new
                {
                    value.ID,
                    value.Name,
                    value.Note,
                    value.CreatedAt,
                    value.LastUpdatedAt
                };

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, mappedCategory);
            }
        }
    }
}
