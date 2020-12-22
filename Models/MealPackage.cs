using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    [JsonConverter(typeof(HSMJsonConverter))]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MealPackage
    {
        public int ID { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }
        public double? Calories { get; set; }
        public double? Protein { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }


        public virtual ICollection<DailyMenu> AssignedMenus { get; set; }
        public virtual ICollection<MealPackageAssignment> MealAssignments { get; set; }

        public IEnumerable<Meal> GetMealsByType(MealType mealType)
        {
            return MealAssignments.Where(a => a.MealType == mealType).Select(a => a.Meal);
        }

        public class HSMJsonConverter : JsonConverter<MealPackage>
        {
            public override MealPackage ReadJson(JsonReader reader, Type objectType, MealPackage existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var package = new MealPackage();
                serializer.Populate(reader, package);

                return package;
            }

            public override void WriteJson(JsonWriter writer, MealPackage value, JsonSerializer serializer)
            {
                var mappedPackage = new
                {
                    value.ID,
                    value.Name,
                    value.Note,
                    value.Calories,
                    value.Protein,
                    BreakfastMeals = value.GetMealsByType(MealType.Breakfast),
                    LunchMeals = value.GetMealsByType(MealType.Lunch),
                    DinnerMeals = value.GetMealsByType(MealType.Dinner),
                    value.CreatedAt,
                    value.LastUpdatedAt
                };

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, mappedPackage);
            }
        }
    }
}
