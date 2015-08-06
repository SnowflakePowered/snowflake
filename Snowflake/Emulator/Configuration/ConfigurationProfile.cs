using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpYaml.Serialization;
using Snowflake.Emulator.Configuration;
namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationProfile : IConfigurationProfile 
    {
        public IDictionary<string, dynamic> ConfigurationValues { get; }
        public string TemplateID { get; }
     
        public ConfigurationProfile (string templateId, IDictionary<string, dynamic> value)
        {
            this.ConfigurationValues = value;
            this.TemplateID = templateId;
        }

        public static IConfigurationProfile FromJsonProtoTemplate (IDictionary<string, dynamic> protoTemplate)
        {
                //Account for JSON source where JObject is required
                return new ConfigurationProfile(protoTemplate["TemplateID"], ((JObject)protoTemplate["ConfigurationValues"])
                    .ToObject<IDictionary<object, dynamic>>()
              .ToDictionary(value => (string)value.Key, value => value.Value));
        }
    }
}
