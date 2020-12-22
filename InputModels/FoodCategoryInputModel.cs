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
    public class FoodCategoryInputModel : IAppEntityInputModel<FoodCategory>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }

        public async Task<FoodCategory> SaveAsEntityAsync(
            int? key,
            AppDbContext context,
            IServiceProvider services
        )
        {
            FoodCategory entity;
            if (key.HasValue)
            {
                entity = await context.FoodCategories.SingleOrDefaultAsync(fc => fc.ID == key.Value);
            }
            else
            {
                entity = new FoodCategory();
            }

            entity.Name = Name;
            entity.Note = Note;


            var saved = context.Update(entity);
            await context.SaveChangesAsync();

            return saved.Entity;
        }
    }
}
