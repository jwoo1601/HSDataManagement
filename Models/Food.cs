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
    public class Food
    {
        public int ID { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }


        public virtual FoodCategory Category { get; set; }
        public virtual ICollection<FoodIngredientAssignment> IngredientAssignments { get; set; }
        public virtual ICollection<MealFoodAssignment> MealAssignments { get; set; }


        public class HSMJsonConverter : JsonConverter<Food>
        {
            public override Food ReadJson(JsonReader reader, Type objectType, Food existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var food = new Food();
                serializer.Populate(reader, food);

                return food;
            }

            public override void WriteJson(JsonWriter writer, Food value, JsonSerializer serializer)
            {
                var mappedFood = new
                {
                    value.ID,
                    value.Name,
                    value.Note,
                    value.Category,
                    Ingredients = value.IngredientAssignments.Select(a => a.Ingredient),
                    value.CreatedAt,
                    value.LastUpdatedAt
                };

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, mappedFood);
            }
        }
    }
}
