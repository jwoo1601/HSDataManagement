using HyosungManagement.Data;
using HyosungManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.InputModels
{
    public class MealPackageInputModel : IAppEntityInputModel<MealPackage>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }
        public double? Calories { get; set; }
        public double? Protein { get; set; }
        [Required]
        public IDictionary<MealType, IEnumerable<int>> Meals { get; set; }

        public async Task<MealPackage> SaveAsEntityAsync(
            int? key,
            AppDbContext context,
            IServiceProvider services
        )
        {
            MealPackage entity;
            if (key.HasValue)
            {
                entity = await context.MealPackages.SingleOrDefaultAsync(mp => mp.ID == key.Value);
            }
            else
            {
                entity = new MealPackage();
            }

            entity.Name = Name;
            entity.Note = Note;
            entity.Calories = Calories;
            entity.Protein = Protein;

            entity.MealAssignments = await AssignExistingMealsAsync(
                context,
                entity
            );

            var saved = context.Update(entity);
            await context.SaveChangesAsync();

            return saved.Entity;
        }

        private async Task<ICollection<MealPackageAssignment>> AssignExistingMealsAsync(
            AppDbContext context,
            MealPackage entity
        )
        {
            var result = new List<MealPackageAssignment>();

            foreach (var mealType in Meals.Keys)
            {
                var existingMeals = (await context.Meals.ToListAsync()).Where(
                    m => Meals[mealType].Contains(m.ID)
                );

                var filteredMeals = new List<Meal>();
                foreach (var category in Enum.GetValues(typeof(MealCategory)).Cast<MealCategory>())
                {
                    // Only allow one meal per category
                    var firstMealInCategory = existingMeals.FirstOrDefault(m => m.Category == category);
                    if (firstMealInCategory != null)
                    {
                        filteredMeals.Add(firstMealInCategory);
                    }
                }
                filteredMeals.Sort((a, b) => a.Category - b.Category);

                result.AddRange(
                    filteredMeals.Select(
                        m => new MealPackageAssignment
                        {
                            Meal = m,
                            MealType = mealType,
                            Package = entity
                        }
                    )
                );
            }

            return result;
        }
    }
}
