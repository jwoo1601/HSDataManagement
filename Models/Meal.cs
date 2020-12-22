using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    public enum MealType
    {
        Breakfast,
        Lunch,
        Dinner
    }

    public enum MealCategory
    {
        Main,
        Dessert,
        SoftMeal,
        Snack
    }

    [JsonConverter(typeof(HSMJsonConverter))]
    public class Meal
    {
        public int ID { get; set; }
        [Required]
        public MealCategory Category { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }

        public virtual ICollection<MealFoodAssignment> FoodAssignments { get; set; }
        public virtual ICollection<MealPackageAssignment> PackageAssignments { get; set; }
        //public virtual ICollection<BreakfastAssignment> BreakfastPackageAssignments { get; set; }
        //public virtual ICollection<LunchAssignment> LunchPackageAssignments { get; set; }
        //public virtual ICollection<DinnerAssignment> DinnerPackageAssignments { get; set; }


        public class HSMJsonConverter : JsonConverter<Meal>
        {
            public override Meal ReadJson(JsonReader reader, Type objectType, Meal existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var meal = new Meal();
                serializer.Populate(reader, meal);

                return meal;
            }

            public override void WriteJson(JsonWriter writer, Meal value, JsonSerializer serializer)
            {
                var mappedMeal = new
                {
                    value.ID,
                    value.Category,
                    value.Name,
                    value.Note,
                    Foods = value.FoodAssignments.Select(a => a.Food),
                    value.CreatedAt,
                    value.LastUpdatedAt
                };

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, mappedMeal);
            }
        }
    }
}
