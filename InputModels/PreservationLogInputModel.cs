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
    public class PreservationLogInputModel : IAppEntityInputModel<PreservationLog>
    {
        [Required]
        public MealType MealType { get; set; }
        [Required]
        public MealCategory MealCategory { get; set; }
        [Required]
        public DateTime LogDate { get; set; }
        [Required]
        public DateTime DateIn { get; set; }
        [Required]
        public DateTime DateOut { get; set; }
        [Required]
        public int Manager { get; set; }

        public async Task<PreservationLog> SaveAsEntityAsync(
            int? key,
            AppDbContext context,
            IServiceProvider services
        )
        {
            var foundMenu = await context.DailyMenus.SingleOrDefaultAsync(dm => dm.ServedDate.Equals(LogDate.Date));
            var foundRole = await context.EmployeeRoles.SingleOrDefaultAsync(er => er.ID == Manager);

            PreservationLog entity;
            if (key.HasValue)
            {
                entity = await context.PreservationLogs.SingleOrDefaultAsync(pr => pr.AssignedMenu.ID == foundMenu.ID);
            }
            else
            {
                entity = new PreservationLog();
            }

            entity.MealType = MealType;
            entity.MealCategory = MealCategory;
            entity.DateIn = DateIn;
            entity.DateOut = DateOut;
            entity.Manager = foundRole;

            var saved = context.Update(entity);
            await context.SaveChangesAsync();

            return saved.Entity;
        }
    }
}
