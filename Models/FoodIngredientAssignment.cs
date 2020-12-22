using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    public class FoodIngredientAssignment
    {
        public int FoodID { get; set; }
        public int FoodIngredientID { get; set; }

        public virtual Food Food { get; set; }
        public virtual FoodIngredient Ingredient { get; set; }
    }
}
