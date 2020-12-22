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
    public class Customer
    {
        public int ID { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; }
        public bool IsHidden { get; set; } = false;
        public bool IsDischarged { get; set; } = false;
        [MaxLength(1000)]
        public string Note { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }
        public bool IsDeleted { get; set; } // soft-delete indicator


        public virtual ICollection<CustomerTagAssignment> TagAssignments { get; set; }
        public virtual ICollection<CustomerServiceAssignment> ServiceAssignments { get; set; }

        public Service GetAssignedServiceByMealType(MealType mealType)
        {
            return ServiceAssignments
                .Where(a => a.MealType == mealType)
                .FirstOrDefault()
                ?.Service;
        }


        public class HSMJsonConverter : JsonConverter<Customer>
        {
            public override Customer ReadJson(JsonReader reader, Type objectType, Customer existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var customer = new Customer();
                serializer.Populate(reader, customer);

                return customer;
            }

            public override void WriteJson(JsonWriter writer, Customer value, JsonSerializer serializer)
            {
                var mappedCustomer = new
                {
                    value.ID,
                    value.Name,
                    value.Note,
                    Hidden = value.IsHidden,
                    Discharged = value.IsDischarged,
                    value.AdmissionDate,
                    value.DischargeDate,
                    value.CreatedAt,
                    value.LastUpdatedAt,
                    Tags = value.TagAssignments.Select(t => t.Tag.Name)
                };

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, mappedCustomer);
            }
        }
    }
}
