using HyosungManagement.Data;
using HyosungManagement.Models;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.InputModels
{
    public class CustomerInputModel : IAppEntityInputModel<Customer>
    {
        [Required, StringLength(30, MinimumLength = 1)]
        public string Name { get; set; }
        public bool Hidden { get; set; }
        public bool Discharged { get; set; }
        [Required]
        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        [Required]
        public IEnumerable<string> Tags { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }

        public async Task<Customer> SaveAsEntityAsync(int? key, AppDbContext context, IServiceProvider services)
        {
            var distinctTags = Tags.Select(t => t.Trim()).Distinct();
            var existingTags = (await context.CustomerTags.ToListAsync()).Where(t => distinctTags.Contains(t.Name));

            var newTags = distinctTags.Except(
                existingTags.Select(t => t.Name)
            ).Select(str => new CustomerTag { Name = str });

            await context.AddRangeAsync(newTags);

            Customer entity;
            if (key.HasValue)
            {
                entity = await context.Customers.SingleOrDefaultAsync(c => c.ID == key.Value);
                if (entity.TagAssignments.Count > 0)
                {
                    context.RemoveRange(entity.TagAssignments);
                }
            }
            else
            {
                entity = new Customer();
            }

            entity.Name = Name;
            entity.IsHidden = Hidden;
            entity.IsDischarged = Discharged;
            entity.AdmissionDate = AdmissionDate;
            entity.DischargeDate = DischargeDate;
            entity.Note = Note;
            entity.TagAssignments = existingTags
                                        .Concat(newTags)
                                        .Select(t => new CustomerTagAssignment { Customer = entity, Tag = t })
                                        .ToList();

            var saved = context.Update(entity);
            await context.SaveChangesAsync();

            return saved.Entity;
        }
    }
}
