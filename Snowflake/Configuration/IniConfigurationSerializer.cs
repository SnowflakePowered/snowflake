﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    public class IniConfigurationSerializer : ConfigurationSerializer
    {
        public bool OutputHeader { get; set; }
       
        public IniConfigurationSerializer(IBooleanMapping booleanMapping, string nullSerializer, bool outputHeader)
            : base(booleanMapping, nullSerializer)
        {
            this.OutputHeader = outputHeader;
        }

        public override string SerializeLine<T>(string key, T value)
        {
            return $"{key} = {this.SerializeValue(value)}";
        }

        public override string SerializeIterableLine<T>(string key, T value, int iteration)
        {
            return $"{key.Replace(ConfigurationSerializer.IteratorKey, iteration.ToString())} = {this.SerializeValue(value)}";
        }

        public override string Serialize(IConfigurationSection configurationSection)
        {
            StringBuilder stringBuilder = new StringBuilder();

            IEnumerable<PropertyInfo> properties = configurationSection.GetType()
                .GetRuntimeProperties()
                .Where(propertyInfo => propertyInfo.IsDefined(typeof (ConfigurationOptionAttribute), true));

            if(this.OutputHeader) stringBuilder.AppendLine($"[{configurationSection.SectionName}]");

            foreach (var prop in properties)
            {
                var data = prop.GetCustomAttribute<ConfigurationOptionAttribute>();
                var value = prop.GetValue(configurationSection);
                stringBuilder.AppendLine(this.SerializeLine(data.OptionName, value));
            }
            return stringBuilder.ToString();
        }

        public override string Serialize(IIterableConfigurationSection iterableConfigurationSection)
        {
            StringBuilder stringBuilder = new StringBuilder();

            IEnumerable<PropertyInfo> properties = iterableConfigurationSection.GetType()
                .GetRuntimeProperties()
                .Where(propertyInfo => propertyInfo.IsDefined(typeof(ConfigurationOptionAttribute), true));

            if (this.OutputHeader) stringBuilder.AppendLine($@"[{iterableConfigurationSection.SectionName
                .Replace(ConfigurationSerializer.IteratorKey, iterableConfigurationSection.InterationNumber.ToString())}]");

            foreach (var prop in properties)
            {
                var data = prop.GetCustomAttribute<ConfigurationOptionAttribute>();
                var value = prop.GetValue(iterableConfigurationSection);
                stringBuilder.AppendLine(data.IsIterable
                    ? this.SerializeIterableLine(data.OptionName, value, iterableConfigurationSection.InterationNumber)
                    : this.SerializeLine(data.OptionName, value));
            }
            return stringBuilder.ToString();
        }
    }
}
