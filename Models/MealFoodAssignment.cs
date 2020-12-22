using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    public class MealFoodAssignment
    {
        public int MealID { get; set; }
        public int FoodID { get; set; }

        [JsonIgnore]
        public virtual Meal Meal { get; set; }
        [JsonIgnore]
        public virtual Food Food { get; set; }
    }
}
