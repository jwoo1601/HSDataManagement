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
    public class FoodInputModel : IAppEntityInputModel<Food>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }
        [Required]
        public int Category { get; set; }
        [Required]
        public IEnumerable<int> Ingredients { get; set; }

        public async Task<Food> SaveAsEntityAsync(
            int? key,
            AppDbContext context,
            IServiceProvider services
        )
        {
            var category = await context.FoodCategories.SingleOrDefaultAsync(c => c.ID == Category);
            if (category == null)
            {
                return null;
            }

            Food entity;
            if (key.HasValue)
            {
                entity = await context.Foods.SingleOrDefaultAsync(f => f.ID == key.Value);
            }
            else
            {
                entity = new Food();
            }

            entity.Name = Name;
            entity.Note = Note;
            entity.Category = category;

            var ingredients = (await context.FoodIngredients.ToListAsync())
                                                        .Where(ig => Ingredients.Contains(ig.ID));

            entity.IngredientAssignments = ingredients.Select(
                ig => new FoodIngredientAssignment
                {
                    Food = entity,
                    Ingredient = ig
                }
            ).ToList();

            var saved = context.Update(entity);
            await context.SaveChangesAsync();

            return saved.Entity;
        }
    }
}
