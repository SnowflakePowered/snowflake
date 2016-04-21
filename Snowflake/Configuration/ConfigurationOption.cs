using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    public class ConfigurationOption : IConfigurationOption
    {

        public string DisplayName { get; }
        public string Description { get; }
        public bool Iterable { get; }
        public bool IsInput { get; }
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
            get
            {
                return this.propertyInfo.GetValue(this.instance);
            }
            set
            {
                this.propertyInfo.SetValue(this.instance, value);
            }
        }


        private readonly PropertyInfo propertyInfo;
        private readonly IConfigurationSection instance;
        internal ConfigurationOption(PropertyInfo propertyInfo, IConfigurationSection instance)
        {
            this.propertyInfo = propertyInfo;
            this.instance = instance;
            this.Type = this.propertyInfo.PropertyType;
            var configOption = this.propertyInfo.GetCustomAttribute<ConfigurationOptionAttribute>();
            this.DisplayName = configOption.DisplayName;
            this.Description = configOption.Description;
            this.Iterable = configOption.Iterable;
            this.IsInput = configOption.IsInput;
            this.IsPath = configOption.IsPath;
            this.Simple = configOption.Simple;
            this.Private = configOption.Private;
            this.Flag = configOption.Flag;
            this.Max = configOption.Max;
            this.Min = configOption.Min;
            this.Increment = configOption.Increment;
            this.OptionName = configOption.OptionName;
            this.KeyName = this.propertyInfo.Name;
        }
    }
}
