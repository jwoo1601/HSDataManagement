using HyosungManagement.Data;
using HyosungManagement.Models;
using HyosungManagement.Models.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.InputModels
{
    public class UserSetRoleInputModel
    {
        [Required]
        public string Role { get; set; }
    }
}
