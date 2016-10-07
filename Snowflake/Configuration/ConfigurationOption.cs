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
using Snowflake.Configuration.Records;

namespace Snowflake.Configuration
{
    public class ConfigurationOption : IConfigurationOption
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
        public Type Type { get; }

        public IConfigurationValue GetValue(Guid withRecord)
            => new ConfigurationValue(propertyInfo, instance, sectionGuid, withRecord);

        public IDictionary<string, object> CustomMetadata { get; }

        private readonly PropertyInfo propertyInfo;
        private readonly object instance;
        private readonly Guid sectionGuid;
        internal ConfigurationOption(PropertyInfo propertyInfo, object instance, Guid sectionGuid)
        {
            this.propertyInfo = propertyInfo;
            this.instance = instance;
            this.sectionGuid = sectionGuid;
            var configOption = propertyInfo.GetCustomAttribute<ConfigurationOptionAttribute>();
            this.Type = propertyInfo.PropertyType;
            this.DisplayName = configOption.DisplayName;
            this.Description = configOption.Description;
            this.IsPath = configOption.IsPath;
            this.Simple = configOption.Simple;
            this.Private = configOption.Private;
            this.Flag = configOption.Flag;
            this.Max = configOption.Max;
            this.Min = configOption.Min;
            this.CustomMetadata =
                propertyInfo.GetCustomAttributes<CustomMetadataAttribute>().ToDictionary(m => m.Key, m => m.Value);
            this.Increment = configOption.Increment;
            this.OptionName = configOption.OptionName;
            this.KeyName = propertyInfo.Name;
        }
    }
}
