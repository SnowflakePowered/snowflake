using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;
using EnumsNET.NonGeneric;

namespace Snowflake.Configuration
{
   
    public class ConfigurationOptionDescriptor : IConfigurationOptionDescriptor
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
        public bool IsSelection { get; }
        public Type Type { get; }
        public IDictionary<string, object> CustomMetadata { get; }
        public ConfigurationOptionType OptionType { get; }
        public IEnumerable<ISelectionOptionDescriptor> SelectionOptions { get; }

        internal ConfigurationOptionDescriptor(ConfigurationOptionAttribute configOption, IEnumerable<CustomMetadataAttribute> customMetadata, string keyName)
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
            this.OptionType = ConfigurationOptionDescriptor.GetOptionType(this.Type, this.IsPath);
            this.IsSelection = this.OptionType == ConfigurationOptionType.Selection;
            this.SelectionOptions = this.IsSelection ? NonGenericEnums.GetMembers(this.Type)
                .Select(m => new SelectionOptionDescriptor(m))
                .ToList() : Enumerable.Empty<ISelectionOptionDescriptor>();
        }

        private static ConfigurationOptionType GetOptionType(Type t, bool isPath)
        {
            if (t.IsEnum)
            {
                return ConfigurationOptionType.Selection;
            }
            if (t == typeof(int) || t == typeof(long))
            {
                return ConfigurationOptionType.Integer;
            }

            if (t == typeof(float) || t == typeof(double))
            {
                return ConfigurationOptionType.Decimal;
            }

            if (t == typeof(bool))
            {
                return ConfigurationOptionType.Boolean;
            }

            if (t == typeof(string))
            {
                if (isPath) return ConfigurationOptionType.Path;
                return ConfigurationOptionType.String;
            }
            throw new ArgumentOutOfRangeException("Option must be a primitive, string, or Enum type.");
        }
    }
}
