using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    public class ConfigurationFile : IConfigurationFile
    {
        public IBooleanMapping BooleanMapping { get; }

        public string Destination { get; }

        public string Key { get; }

        public ConfigurationFile(string fileName, string key, IBooleanMapping booleanMapping)
        {
            this.Destination = fileName;
            this.Key = key;
            this.BooleanMapping = booleanMapping;
        }
    }
}
