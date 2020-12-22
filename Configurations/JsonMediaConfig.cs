using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.Configurations
{
    public static class JsonMediaConfig
    {
        public static IMvcBuilder AddJsonConfig(this IMvcBuilder builder)
        {
            return builder.AddNewtonsoftJson(options => {
                InjectSettings(options.SerializerSettings);
            });
        }

        public static void InjectSettings(JsonSerializerSettings settings)
        {
            settings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy(true, true, true),
            };
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.Converters = new JsonConverter[] {
                new StringEnumConverter(typeof(SnakeCaseNamingStrategy)),
                new IsoDateTimeConverter()
            };
            settings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
        }
    }
}
