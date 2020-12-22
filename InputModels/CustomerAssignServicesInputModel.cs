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
    public class CustomerAssignServicesInputModel : IAppEntityInputModel<Customer>
    {
        [Required]
        public IDictionary<MealType, int> Services { get; set; }

        public async Task<Customer> SaveAsEntityAsync(int? key, AppDbContext context, IServiceProvider services)
        {
            if (!key.HasValue)
            {
                return null;
            }

            var customer = await context.Customers.SingleOrDefaultAsync(c => c.ID == key.Value);

            foreach (var mealType in Enum.GetValues(typeof(MealType)).Cast<MealType>())
            {
                if (!Services.ContainsKey(mealType))
                {
                    continue;
                }

                var service = await context.Services.SingleOrDefaultAsync(s => s.ID == Services[mealType]);
                if (service == null)
                {
                    continue;
                }

                customer.ServiceAssignments.Add(
                    new CustomerServiceAssignment
                    {
                        Customer = customer,
                        Service = service,
                        MealType = mealType
                    }
                );
            }

            context.Update(customer);
            await context.SaveChangesAsync();

            return customer;
        }
    }
}
