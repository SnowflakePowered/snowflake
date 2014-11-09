using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Configuration.Mapping;
using Snowflake.Emulator.Configuration.Type;
using Snowflake.Emulator.Configuration.Template;

using SharpYaml.Serialization;

namespace Snowflake.Emulator.Configuration
{
    public class EmulatorConfiguration 
    {

        public IDictionary<string, dynamic> keys;
        public string Compile(ConfigurationTemplate template)
        {
            var builder = new StringBuilder(template.StringTemplate);
            foreach (KeyValuePair<string, dynamic> value in this.keys)
            {
                var entry = template.ConfigurationEntries[value.Key];
                string input;
                switch (entry.Type)
                {
                    case "boolean":
                        try
                        {
                            input = template.BooleanMapping.FromBool((bool) value.Value);
                        }catch
                        {
                            input = "false";
                        }
                        break;
                    default:
                        input = value.Value.ToString();
                        break;
                }

                builder.Replace("{" + value.Key + "}", input);
            }
            return builder.ToString();
        }

        public EmulatorConfiguration()
        {
            this.keys = new Dictionary<string, dynamic>();
        }
    }
}
