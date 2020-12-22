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
    public class OperationLogInputModel : IAppEntityInputModel<OperationLog>
    {
        [Required]
        public MealType MealType { get; set; }
        [Required]
        public DateTime LogDate { get; set; }
        [Required]
        public int NumCustomersServed { get; set; }
        [Required]
        public int NumEmployeesServed { get; set; }

        public async Task<OperationLog> SaveAsEntityAsync(
            int? key,
            AppDbContext context,
            IServiceProvider services
        )
        {
            var foundMenu = await context.DailyMenus.SingleOrDefaultAsync(dm => dm.ServedDate.Equals(LogDate.Date));

            OperationLog entity;
            if (key.HasValue)
            {
                entity = await context.OperationLogs.SingleOrDefaultAsync(op => op.DailyMenu.ID == foundMenu.ID);
            }
            else
            {
                entity = new OperationLog();
            }

            entity.MealType = MealType;
            entity.NumCustomersServed = NumCustomersServed;
            entity.NumEmployeesServed = NumEmployeesServed;
            entity.DailyMenu = foundMenu;

            var saved = context.Update(entity);
            await context.SaveChangesAsync();

            return saved.Entity;
        }
    }
}
