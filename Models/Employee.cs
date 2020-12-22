using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    [JsonConverter(typeof(HSMJsonConverter))]
    public class Employee
    {
        public int ID { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }
        [Required]
        public DateTime LicensedDate { get; set; }
        public DateTime? LicenseRenewalDate { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }


        public virtual EmployeeRole Role { get; set; }


        public class HSMJsonConverter : JsonConverter<Employee>
        {
            public override Employee ReadJson(JsonReader reader, Type objectType, Employee existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var employee = new Employee();
                serializer.Populate(reader, employee);

                return employee;
            }

            public override void WriteJson(JsonWriter writer, Employee value, JsonSerializer serializer)
            {
                var mappedEmployee = new
                {
                    value.ID,
                    value.Role.DisplayName,
                    value.LicensedDate,
                    value.LicenseRenewalDate,
                };

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, mappedEmployee);
            }
        }
    }
}
