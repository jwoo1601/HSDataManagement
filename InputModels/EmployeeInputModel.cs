using HyosungManagement.Data;
using HyosungManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.InputModels
{
    public class EmployeeInputModel : IAppEntityInputModel<Employee>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }
        [Required]
        public int Role { get; set; }
        [Required]
        public DateTime LicensedDate { get; set; }

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
