using HyosungManagement.Data;
using HyosungManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.InputModels
{
    public class EmployeeLicenseRenewalInputModel : IAppEntityInputModel<Employee>
    {
        [Required]
        public DateTime RenewalDate { get; set; }

        public async Task<Employee> SaveAsEntityAsync(
            int? key,
            AppDbContext context,
            IServiceProvider services
        )
        {
            throw new NotImplementedException();
        }
    }
}
