using HyosungManagement.Data;
using HyosungManagement.Models;
using HyosungManagement.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.InputModels
{
    public class SecurityCodeGenerateInputModel : IUserDbEntityInputModel<SecurityCode>
    {
        public static readonly int DefaultAgeInSeconds = 3600 * 24; // 1 day

        public SecurityCodeType? CodeType { get; set; }
        [JsonProperty("age")]
        [Range(0, int.MaxValue)]
        public int? AgeInSeconds { get; set; }

        public async Task<SecurityCode> SaveAsEntityAsync(
            int? key,
            UserDbContext context,
            IServiceProvider services
        )
        {
            var now = DateTime.UtcNow;
            var entity = new SecurityCode
            {
                CodeType = CodeType ?? SecurityCodeType.Normal,
                IsValid = true
            };

            if (entity.CodeType != SecurityCodeType.Persistent)
            {
                entity.ExpiresAt = now + TimeSpan.FromSeconds(AgeInSeconds ?? DefaultAgeInSeconds);
            }

            var generator = services.GetRequiredService<IRandomCodeGenerator>();
            entity.Value = generator.GenerateRandomPasscode(
                new PasscodeGenerationHints
                {
                    IsUnique = false,
                    NumBytes = 32,
                    TextFormat = PasscodeTextFormat.Base64
                }
            );

            var saved = await context.SecurityCodes.AddAsync(entity);
            await context.SaveChangesAsync();

            return saved.Entity;
        }
    }
}
