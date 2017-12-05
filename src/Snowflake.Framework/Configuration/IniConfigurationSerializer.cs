﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

namespace Snowflake.Configuration
{
    /// <summary>
    /// A configuration serializer that uses INI format
    ///
    /// [Header]
    /// Key = Value
    /// </summary>
    public class IniConfigurationSerializer : ConfigurationSerializer
    {
        public IniConfigurationSerializer(IBooleanMapping booleanMapping, string nullSerializer)
            : base(booleanMapping, nullSerializer)
        {
        }

        /// <inheritdoc/>
        public override string SerializeLine<T>(string key, T value)
        {
            return $"{key} = {this.SerializeValue(value)}";
        }

        /// <inheritdoc/>
        public override string SerializeHeader(string headerString) => $"[{headerString}]{Environment.NewLine}";

        /// <inheritdoc/>
        public override string Serialize(IConfigurationSection configurationSection)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(this.SerializeHeader(configurationSection.Descriptor.SectionName));
            foreach (var config in from option in configurationSection where !option.Key.Flag select option)
            {
                stringBuilder.AppendLine(this.SerializeLine(config.Key.OptionName, config.Value.Value));
            }

            return stringBuilder.ToString();
        }
    }
}
