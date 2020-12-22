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
    public class ServiceGroupInputModel : IAppEntityInputModel<ServiceGroup>
    {
        [Required, MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }

        public async Task<ServiceGroup> SaveAsEntityAsync(
            int? key,
            AppDbContext context,
            IServiceProvider services
        )
        {
            ServiceGroup entity;
            if (key.HasValue)
            {
                entity = await context.ServiceGroups.SingleOrDefaultAsync(sg => sg.ID == key.Value);
            }
            else
            {
                entity = new ServiceGroup();
            }

            entity.Name = Name;
            entity.Note = Note;

            var saved = context.Update(entity);
            await context.SaveChangesAsync();

            return saved.Entity;
        }
    }
}
