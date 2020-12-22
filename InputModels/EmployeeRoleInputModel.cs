using HyosungManagement.Data;
using HyosungManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.InputModels
{
    public class EmployeeRoleInputModel : IAppEntityInputModel<EmployeeRole>
    {
        [Required, MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(30)]
        public string DisplayName { get; set; }
        [MaxLength(500)]
        public string Note { get; set; }

        public async Task<EmployeeRole> SaveAsEntityAsync(
            int? key,
            AppDbContext context,
            IServiceProvider services
        )
        {
            throw new NotImplementedException();
        }
    }
}
