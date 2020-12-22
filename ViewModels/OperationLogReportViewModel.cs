using HyosungManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.ViewModels
{
    public class OperationLogReportViewModel : ReportViewModel
    {
        public DailyMenu Menu { get; set; }
        public IEnumerable<Customer> Customers { get; set; }

        public OperationLogReportViewModel()
            : base(3, "name.operationLog")
        {

        }

        public IEnumerable<Meal> GetMealsByType(MealType mealType)
        {
            return Menu.Package.GetMealsByType(mealType);
        }

        public IEnumerable<Food> GetFoodsByType(MealType mealType)
        {
            return GetMealsByType(mealType)
                .SelectMany(m => m.FoodAssignments.Select(a => a.Food));
        }

        public IEnumerable<Food> GetFoodsInCategory(MealType mealType, MealCategory category)
        {
            return GetMealsByType(mealType)
                .Where(m => m.Category == category)
                .SelectMany(m => m.FoodAssignments.Select(a => a.Food));
        }
    }
}
