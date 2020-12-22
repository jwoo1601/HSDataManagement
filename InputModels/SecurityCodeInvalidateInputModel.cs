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
    public class SecurityCodeInvalidateInputModel : IUserEntityInputModel<SecurityCode>
    {
        [Required]
        public string Code { get; set; }

        public async Task<SecurityCode> SaveAsEntityAsync(
            int? key,
            UserDbContext context,
            IServiceProvider services
        )
        {
            var entity = await context.SecurityCodes.SingleOrDefaultAsync(sc => sc.Value.Equals(Code));
            if (entity == null)
            {
                return null;
            }

            entity.IsValid = false;
            var saved = context.Update(entity);
            await context.SaveChangesAsync();

            return saved.Entity;
        }
    }
}
