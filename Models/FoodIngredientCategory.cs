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
    public class FoodIngredientCategory
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


        public virtual ICollection<FoodIngredient> Ingredients { get; set; }


        public class HSMJsonConverter : JsonConverter<FoodIngredientCategory>
        {
            public override FoodIngredientCategory ReadJson(JsonReader reader, Type objectType, FoodIngredientCategory existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var category = new FoodIngredientCategory();
                serializer.Populate(reader, category);

                return category;
            }

            public override void WriteJson(JsonWriter writer, FoodIngredientCategory value, JsonSerializer serializer)
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
