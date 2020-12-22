using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models
{
    public enum MenuLogType
    {
        Operation,
        Preservation,
        Inspection
    }

    [JsonConverter(typeof(HSMJsonConverter))]
    public class DailyMenu
    {
        public int ID { get; set; }
        [Required]
        public DateTime ServedDate { get; set; }
        [MaxLength(1000)]
        public string Note { get; set; }
        //[Required]
        //public int PackageID { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }


        public virtual MealPackage Package { get; set; }
        public virtual ICollection<OperationLog> OperationLogs { get; set; }
        public virtual ICollection<PreservationLog> PreservationLogs { get; set; }


        public class HSMJsonConverter : JsonConverter<DailyMenu>
        {
            public override DailyMenu ReadJson(JsonReader reader, Type objectType, DailyMenu existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var menu = new DailyMenu();
                serializer.Populate(reader, menu);

                return menu;
            }

            public override void WriteJson(JsonWriter writer, DailyMenu value, JsonSerializer serializer)
            {
                var mappedMenu = new
                {
                    value.ID,
                    value.ServedDate,
                    value.Note,
                    value.Package,
                    value.CreatedAt,
                    value.LastUpdatedAt
                    //Operations = value.OperationLogs,
                    //Preservations = value.PreservationLogs
                };

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, mappedMenu);
            }
        }
    }
}
