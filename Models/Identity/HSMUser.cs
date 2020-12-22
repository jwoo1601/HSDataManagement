using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Models.Identity
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(HSMJsonConverter))]
    public class HSMUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        public string Locale { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime RegisteredAt { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedAt { get; set; }
        public bool IsActive { get; set; }


        public virtual SecurityCode SecurityCode { get; set; }
        public virtual ICollection<HSMUserRole> Roles { get; set; }
        public virtual ICollection<HSMUserClaim> Claims { get; set; }
        public virtual ICollection<HSMUserLogin> Logins { get; set; }
        public virtual ICollection<HSMUserToken> Tokens { get; set; }


        public class HSMJsonConverter : JsonConverter<HSMUser>
        {
            public override HSMUser ReadJson(JsonReader reader, Type objectType, HSMUser existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var user = new HSMUser();
                serializer.Populate(reader, user);

                return user;
            }

            public override void WriteJson(JsonWriter writer, HSMUser value, JsonSerializer serializer)
            {
                if (value == null)
                {
                    writer.WriteNull();
                    return;
                }

                var mappedUser = new
                {
                    value.Id,
                    Username = value.UserName,
                    value.Email,
                    value.EmailConfirmed,
                    value.Name,
                    value.SecurityCode,
                    value.Locale,
                    value.AccessFailedCount,
                    value.LockoutEnd,
                    value.RegisteredAt,
                    value.LastUpdatedAt,
                    value.IsActive,
                    Roles = value.Roles?.Select(r => r.Role) ?? Enumerable.Empty<HSMRole>()
                };

                serializer.Serialize(writer, mappedUser);
            }
        }
    }
}
