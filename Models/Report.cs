using HyosungManagement.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    public enum ReportType
    {
        MonthlyPlan = 1,
        WeeklyMenu,
        CookingPlan,
        OperationLog,
        InspectionLog,
        FoodSafetyTrainingLog,
        GasInspectionLog,

        LicenseStatus = 100,
        WeeklyMenuDisplay,
        PreservationLog,
        SatisfactionSurvey
    }

    public class Report
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        public ReportType ReportType { get; set; }
        [Required]
        public DateTime LogDate { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }
        public string GeneratedBy { get; set; }
    }
}
