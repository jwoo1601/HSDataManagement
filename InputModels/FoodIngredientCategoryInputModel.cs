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
    public class FoodIngredientCategoryInputModel : IAppEntityInputModel<FoodIngredientCategory>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }

        public async Task<FoodIngredientCategory> SaveAsEntityAsync(
            int? key,
            AppDbContext context,
            IServiceProvider services
        )
        {
            FoodIngredientCategory entity;
            if (key.HasValue)
            {
                entity = await context.FoodIngredientCategories.SingleOrDefaultAsync(fic => fic.ID == key.Value);
            }
            else
            {
                entity = new FoodIngredientCategory();
            }

            entity.Name = Name;
            entity.Note = Note;

            var saved = context.Update(entity);
            await context.SaveChangesAsync();

            return saved.Entity;
        }
    }
}
