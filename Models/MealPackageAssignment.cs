using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    public class MealPackageAssignment
    {
        public MealType MealType { get; set; }
        public int MealID { get; set; }
        public int PackageID { get; set; }

        [JsonIgnore]
        public virtual Meal Meal { get; set; }
        [JsonIgnore]
        public virtual MealPackage Package { get; set; }
    }

    //public class BreakfastAssignment : MealPackageAssignment { }
    //public class LunchAssignment : MealPackageAssignment { }
    //public class DinnerAssignment : MealPackageAssignment { }
}
