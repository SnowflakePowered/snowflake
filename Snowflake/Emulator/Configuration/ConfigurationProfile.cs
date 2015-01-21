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
        public IReadOnlyDictionary<string, dynamic> ConfigurationValues { get; private set; }
        public string TemplateID { get; private set; }
     
        public ConfigurationProfile (string templateId, IDictionary<string, dynamic> value)
        {
            this.ConfigurationValues = value.AsReadOnly();
            this.TemplateID = templateId;
        }

        public static ConfigurationProfile FromDictionary (IDictionary<string, dynamic> protoTemplate)
        {
            try
            {
                return new ConfigurationProfile(protoTemplate["TemplateID"], ((IDictionary<object, dynamic>)protoTemplate["ConfigurationValues"])
                .ToDictionary(value => (string)value.Key, value => value.Value));
            }
            catch (InvalidCastException)
            {
                //Account for JSON source where JObject is required
                return new ConfigurationProfile(protoTemplate["TemplateID"], ((JObject)protoTemplate["ConfigurationValues"])
                    .ToObject<IDictionary<object, dynamic>>()
              .ToDictionary(value => (string)value.Key, value => value.Value));
            }
        }
    }
}
