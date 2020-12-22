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
    public class DailyMenuInputModel : IAppEntityInputModel<DailyMenu>
    {
        [Required]
        public DateTime ServedDate { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }
        [Required]
        public int Package { get; set; }

        public async Task<DailyMenu> SaveAsEntityAsync(
            int? key,
            AppDbContext context,
            IServiceProvider services
        )
        {
            var foundPackage = await context.MealPackages.SingleOrDefaultAsync(mp => mp.ID == Package);

            DailyMenu entity;
            if (key.HasValue)
            {
                entity = await context.DailyMenus.SingleOrDefaultAsync(dm => dm.ID == key.Value);
            }
            else
            {
                entity = new DailyMenu();
            }

            entity.ServedDate = ServedDate;
            entity.Note = Note;
            entity.Package = foundPackage;

            var saved = context.Update(entity);
            await context.SaveChangesAsync();

            return saved.Entity;
        }
    }
}
