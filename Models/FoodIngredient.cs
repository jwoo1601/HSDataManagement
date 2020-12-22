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
    public class FoodIngredient
    {
        public int ID { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Origin { get; set; }
        //public int CategoryID { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }


        public virtual FoodIngredientCategory Category { get; set; }
        public virtual ICollection<FoodIngredientAssignment> FoodAssignments { get; set; }


        public class HSMJsonConverter : JsonConverter<FoodIngredient>
        {
            public override FoodIngredient ReadJson(JsonReader reader, Type objectType, FoodIngredient existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var ingredient = new FoodIngredient();
                serializer.Populate(reader, ingredient);

                return ingredient;
            }

            public override void WriteJson(JsonWriter writer, FoodIngredient value, JsonSerializer serializer)
            {
                var mappedIngredient = new
                {
                    value.ID,
                    value.Name,
                    value.Origin,
                    value.Category,
                    value.CreatedAt,
                    value.LastUpdatedAt
                };

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, mappedIngredient);
            }
        }
    }
}
