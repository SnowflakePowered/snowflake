using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    public class ConfigurationFile : IConfigurationFile
    {
        /// <inheritdoc/>
        public IBooleanMapping BooleanMapping { get; }

        /// <inheritdoc/>
        public string Destination { get; }

        /// <inheritdoc/>
        public string Key { get; }

        public ConfigurationFile(string fileName, string key, IBooleanMapping booleanMapping)
        {
            this.Destination = fileName;
            this.Key = key;
            this.BooleanMapping = booleanMapping;
        }
    }
}
