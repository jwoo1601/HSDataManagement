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
    public class FoodIngredientInputModel : IAppEntityInputModel<FoodIngredient>
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Origin { get; set; }
        [Required]
        public int Category { get; set; }

        public async Task<FoodIngredient> SaveAsEntityAsync(
            int? key,
            AppDbContext context,
            IServiceProvider services
        )
        {
            var category = await context.FoodIngredientCategories.SingleOrDefaultAsync(c => c.ID == Category);
            if (category == null)
            {
                return null;
            }

            FoodIngredient entity;
            if (key.HasValue)
            {
                entity = await context.FoodIngredients.SingleOrDefaultAsync(fi => fi.ID == key.Value);
            }
            else
            {
                entity = new FoodIngredient();
            }

            entity.Name = Name;
            entity.Origin = Origin;
            entity.Category = category;

            var saved = context.Update(entity);
            await context.SaveChangesAsync();

            return saved.Entity;
        }
    }
}
