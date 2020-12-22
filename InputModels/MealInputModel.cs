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
    public class MealInputModel : IAppEntityInputModel<Meal>
    {
        [Required]
        public MealCategory Category { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }
        [Required]
        public IEnumerable<int> Foods { get; set; }

        public async Task<Meal> SaveAsEntityAsync(
            int? key,
            AppDbContext context,
            IServiceProvider services
        )
        {
            Meal entity;
            if (key.HasValue)
            {
                entity = await context.Meals.SingleOrDefaultAsync(m => m.ID == key.Value);
            }
            else
            {
                entity = new Meal();
            }

            entity.Category = Category;
            entity.Name = Name;
            entity.Note = Note;

            var foods = (await context.Foods.ToListAsync()).Where(f => Foods.Contains(f.ID));
            entity.FoodAssignments = foods.Select(
                f => new MealFoodAssignment
                {
                    Meal = entity,
                    Food = f
                }
            ).ToList();

            var saved = context.Update(entity);
            await context.SaveChangesAsync();

            return saved.Entity;
        }
    }
}
