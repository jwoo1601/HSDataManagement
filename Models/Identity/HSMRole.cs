using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models.Identity
{
    [JsonConverter(typeof(HSMJsonConverter))]
    public class HSMRole : IdentityRole
    {
        public HSMRole()
        {

        }

        public HSMRole(string roleName) : base(roleName)
        {
        }


        public virtual ICollection<HSMUserRole> Users { get; set; }
        public virtual ICollection<HSMRoleClaim> Claims { get; set; }


        public class HSMJsonConverter : JsonConverter<HSMRole>
        {
            public override HSMRole ReadJson(JsonReader reader, Type objectType, HSMRole existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var role = new HSMRole();
                serializer.Populate(reader, role);

                return role;
            }

            public override void WriteJson(JsonWriter writer, HSMRole value, JsonSerializer serializer)
            {
                var mappedRole = new
                {
                    value.Id,
                    value.Name
                };

                serializer.Serialize(writer, mappedRole);
            }
        }
    }
}
