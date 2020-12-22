﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    public class EmployeeRole
    {
        public int ID { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(30)]
        public string DisplayName { get; set; }
        [MaxLength(500)]
        public string Note { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }


        public virtual ICollection<Employee> Employees { get; set; }
    }
}
