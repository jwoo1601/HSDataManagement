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
    public class ServiceInputModel : IAppEntityInputModel<Service>
    {
        [Required, MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Note { get; set; }
        public TimeSpan? Duration { get; set; }
        public int? Group { get; set; }

        public async Task<Service> SaveAsEntityAsync(
            int? key,
            AppDbContext context,
            IServiceProvider services
        )
        {
            var group = await context.ServiceGroups.SingleOrDefaultAsync(sg => sg.ID == Group);

            Service entity;
            if (key.HasValue)
            {
                entity = await context.Services.SingleOrDefaultAsync(s => s.ID == key.Value);
            }
            else
            {
                entity = new Service();
            }

            entity.Name = Name;
            entity.Note = Note;
            entity.Duration = Duration;
            entity.Group = group;

            var saved = context.Update(entity);
            await context.SaveChangesAsync();

            return saved.Entity;
        }
    }
}
