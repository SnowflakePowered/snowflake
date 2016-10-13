using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FastMember;
using Newtonsoft.Json;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;

namespace Snowflake.DynamicConfiguration
{
    public class ConfigurationOption 
    {

        public string DisplayName { get; }
        public string Description { get; }
        public bool Simple { get; }
        public bool Private { get; }
        public bool Flag { get; }
        public double Max { get; }
        public double Min { get; }
        public double Increment { get; }
        public bool IsPath { get; }
        public string OptionName { get; }
        public string KeyName { get; }
        public object Default { get; }
        public Type Type { get; }
        public IDictionary<string, object> CustomMetadata { get; }

        internal ConfigurationOption(ConfigurationOptionAttribute configOption, IEnumerable<CustomMetadataAttribute> customMetadata, string keyName)
        {
            this.Default = configOption.Default;
            this.Type = configOption.Default.GetType();
            this.DisplayName = configOption.DisplayName;
            this.Description = configOption.Description;
            this.IsPath = configOption.IsPath;
            this.Simple = configOption.Simple;
            this.Private = configOption.Private;
            this.Flag = configOption.Flag;
            this.Max = configOption.Max;
            this.Min = configOption.Min;
            this.CustomMetadata = customMetadata.ToDictionary(m => m.Key, m => m.Value);
            this.Increment = configOption.Increment;
            this.OptionName = configOption.OptionName;
            this.KeyName = keyName;
        }
    }
}
