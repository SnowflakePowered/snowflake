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

        [JsonIgnore]
        public object Value
        {
            get { return this.accessor[this.instance, this.KeyName]; }
            set { setter(value); }
        }

        public IDictionary<string, object> CustomMetadata { get; }

        private readonly object instance;
        private readonly TypeAccessor accessor;
        private readonly Action<object> setter;
        internal ConfigurationOption(PropertyInfo propertyInfo, object instance) 
        {
             this.instance = instance;
            this.setter = value =>
            {
                if (value != null) this.accessor[this.instance, this.KeyName] = value;
                else propertyInfo.SetValue(this.instance, null); // setting a value to null will incur nre unless with reflection
            };
            this.Type = propertyInfo.PropertyType;
            this.accessor = TypeAccessor.Create(instance.GetType());
            var configOption = propertyInfo.GetCustomAttribute<ConfigurationOptionAttribute>();
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
