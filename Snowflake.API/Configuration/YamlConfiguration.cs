using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using Snowflake.API.Interface;
using SharpYaml.Serialization;
using System.IO;

namespace Snowflake.API.Configuration
{
    public class YamlConfiguration : IConfiguration
    {
        public string ConfigurationFileName { get; private set; }
        public Dictionary<string, dynamic> Configuration { get; private set; }
      
        public YamlConfiguration(string configFileName)
        {
            this.ConfigurationFileName = configFileName;
            this.Configuration = new Dictionary<string, dynamic>();
        }
        public void LoadConfiguration()
        {
            var serializer = new Serializer(new SerializerSettings()
            {
                EmitTags = false
            });
            string serializedYaml = File.ReadAllText(this.ConfigurationFileName);
            this.Configuration = serializer.Deserialize<Dictionary<string, dynamic>>(serializedYaml);
            
        }

        public void SaveConfiguration()
        {
            var serializer = new Serializer(new SerializerSettings()
            {
                EmitTags = false
            });
            string serializedYaml = serializer.Serialize(this.Configuration);
            File.WriteAllText(ConfigurationFileName, serializedYaml);
        }
    }
}
