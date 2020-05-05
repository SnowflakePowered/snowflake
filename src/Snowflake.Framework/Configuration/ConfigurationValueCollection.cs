using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;
using Snowflake.Configuration.Utility;
using System.Linq.Expressions;
using Snowflake.Configuration.Extensions;
using System.Reflection;
using System.ComponentModel;
using EnumsNET;

namespace Snowflake.Configuration
{
    public class ConfigurationValueCollection : IConfigurationValueCollection
    {
        private IDictionary<string, Dictionary<string, IConfigurationValue>> BackingDictionary { get; }
        private IDictionary<Guid, (string section, string option, IConfigurationValue value)> ValueBackingDictionary { get; }

        /// <summary>
        /// Cache of ensured descriptors
        /// </summary>
        private HashSet<string> EnsuredDescriptors { get; }

        internal ConfigurationValueCollection()
            : this(Enumerable.Empty<(string section, string option, IConfigurationValue value)>())
        {
        }

        internal ConfigurationValueCollection(IEnumerable<(string section, string option, IConfigurationValue)> values)
            : this(values, Guid.NewGuid())
        {
        }

        internal static object FromString(string? strValue, Type? optionType)
        {
            return optionType == typeof(string)
                ? strValue ?? string.Empty // return string value if string
                : TypeDescriptor.GetConverter(optionType).ConvertFromInvariantString(strValue);

            //return optionType == typeof(string)
            //    ? strValue ?? string.Empty // return string value if string
            //    : optionType?.GetTypeInfo()?.IsEnum ?? false
            //        ? Enums.Parse(optionType!, strValue ?? String.Empty) // return parsed enum if enum
            //        : TypeDescriptor.GetConverter(optionType).ConvertFromInvariantString(strValue);
        }

        public static IConfigurationValueCollection MakeExistingValueCollection<T>
        (IEnumerable<(string section, string option, (string stringValue, Guid guid, ConfigurationOptionType type) value)> values,
            Guid collectionGuid)
            where T : class, IConfigurationCollection, IConfigurationCollection<T>
        {
            var typedValues = new List<(string, string, IConfigurationValue)>();
            foreach ((string section, string option, (string stringValue, Guid guid, ConfigurationOptionType type) value) in values)
            {
                Type type = value.type switch
                {
                    ConfigurationOptionType.Boolean => typeof(bool),
                    ConfigurationOptionType.String => typeof(string),
                    ConfigurationOptionType.Path => typeof(string),
                    ConfigurationOptionType.Integer => typeof(long),
                    ConfigurationOptionType.Decimal => typeof(double),
                    ConfigurationOptionType.Selection => typeof(int),
                    _ => throw new NotImplementedException(),
                };

                typedValues.Add((section, option,
                        new ConfigurationValue(FromString(value.stringValue, type), value.guid, value.type)));
            }
            return new ConfigurationValueCollection(typedValues, collectionGuid);
        }

        public static IConfigurationValueCollection MakeExistingValueCollection<T>
        (IEnumerable<(string option, (string stringValue, Guid guid, ConfigurationOptionType type) value)> values,
            string sectionName,
            Guid collectionGuid)
            where T : class, IConfigurationSection<T>
        {
            var typedValues = new List<(string, string, IConfigurationValue)>();
            var descriptor = new ConfigurationSectionDescriptor<T>(sectionName);
           
            foreach (var (option, value) in values)
            {
                //Type? type = descriptor?[option]?.Type;
                Type type = value.type switch
                {
                    ConfigurationOptionType.Boolean => typeof(bool),
                    ConfigurationOptionType.String => typeof(string),
                    ConfigurationOptionType.Path => typeof(string),
                    ConfigurationOptionType.Integer => typeof(int),
                    ConfigurationOptionType.Decimal => typeof(double),
                    ConfigurationOptionType.Selection => typeof(int),
                    _ => throw new NotImplementedException(),
                };
                typedValues.Add((sectionName, option,
                    new ConfigurationValue(FromString(value.stringValue, type), value.guid, value.type)));
            }

            return new ConfigurationValueCollection(typedValues, collectionGuid);
        }

        internal ConfigurationValueCollection(
            IEnumerable<(string section, string option, IConfigurationValue value)> values, Guid guid)
        {
            this.Guid = guid;
            var defs = values.GroupBy(p => p.section)
                .ToDictionary(p => p.Key, p => p.ToDictionary(o => o.option, k => k.value));
            this.BackingDictionary = defs;
            this.ValueBackingDictionary = values.ToDictionary(p => p.value.Guid, p => p);
            if (this.BackingDictionaryValueCount != this.ValueBackingDictionary.Count)
                throw new InvalidOperationException("Number of stored configuration values is inconsistent.");
            this.EnsuredDescriptors = new HashSet<string>();
        }

        public IReadOnlyDictionary<string, IConfigurationValue> this[IConfigurationSectionDescriptor descriptor]
        {
            get
            {
                this.EnsureSectionDefaults(descriptor);
                return ImmutableDictionary.CreateRange(this.BackingDictionary[descriptor.SectionKey]);
            }
        }

        internal void EnsureSectionDefaults(IConfigurationSectionDescriptor descriptor)
        {
            if (this.EnsuredDescriptors.Contains(descriptor.SectionKey)) return;
            this.EnsuredDescriptors.Add(descriptor.SectionKey);

            if (!this.BackingDictionary.ContainsKey(descriptor.SectionKey))
            {
                this.BackingDictionary[descriptor.SectionKey] = new Dictionary<string, IConfigurationValue>();
            }

            foreach (var prop in descriptor.Options)
            {
                if (!this.BackingDictionary[descriptor.SectionKey].ContainsKey(prop.OptionKey))
                {
                    var newConfigurationValue = new ConfigurationValue(prop.Default, prop.OptionType);
                    this.BackingDictionary[descriptor.SectionKey][prop.OptionKey] = newConfigurationValue;
                    this.ValueBackingDictionary[newConfigurationValue.Guid] = (descriptor.SectionKey, prop.OptionKey, newConfigurationValue);
                } 
                else if (this.BackingDictionary[descriptor.SectionKey][prop.OptionKey].Type 
                    == ConfigurationOptionType.Selection)
                {
                    // ensure selection
                    var enumType = descriptor[prop.OptionKey].Type;
                    this.BackingDictionary[descriptor.SectionKey][prop.OptionKey].Value = Enums.GetMember(enumType, this.BackingDictionary[descriptor.SectionKey][prop.OptionKey].Value)?.Value ?? 0;
                }
            }
            if (descriptor.Options.Any() && this.BackingDictionaryValueCount != this.ValueBackingDictionary.Count)
                throw new InvalidOperationException("Number of stored configuration values is inconsistent after ensuring descriptor.");
        }

        internal void EnsureSectionDefault(IConfigurationSectionDescriptor descriptor, string option)
        {
            if (this.EnsuredDescriptors.Contains(descriptor.SectionKey)) return;

            if (!this.BackingDictionary.ContainsKey(descriptor.SectionKey))
            {
                this.BackingDictionary[descriptor.SectionKey] = new Dictionary<string, IConfigurationValue>();
            }

            var key = descriptor.Options.FirstOrDefault(o => o.OptionKey == option);

            if (key != null
                && !this.BackingDictionary[descriptor.SectionKey].ContainsKey(option))
            {
                var newConfigurationValue = new ConfigurationValue(key.Default, key.OptionType);
                this.BackingDictionary[descriptor.SectionKey][key.OptionKey] = newConfigurationValue;
                this.ValueBackingDictionary[newConfigurationValue.Guid] = (descriptor.SectionKey, key.OptionKey, newConfigurationValue);
                if (this.BackingDictionaryValueCount != this.ValueBackingDictionary.Count)
                    throw new InvalidOperationException("Number of stored configuration values is inconsistent after ensuring descriptor.");
            }
            else if (this.BackingDictionary[descriptor.SectionKey][option].Type
                  == ConfigurationOptionType.Selection)
            {
                var enumType = descriptor[option].Type;
                this.BackingDictionary[descriptor.SectionKey][option].Value = 
                    Enums.GetMember(enumType, this.BackingDictionary[descriptor.SectionKey][option].Value)?.Value ?? 0;
            }
        }

        public IConfigurationValue? this[IConfigurationSectionDescriptor descriptor, string option]
        {
            get
            {
                this.EnsureSectionDefault(descriptor, option);
                if (!this.BackingDictionary[descriptor.SectionKey].ContainsKey(option)) return null;
                return this.BackingDictionary[descriptor.SectionKey][option];
            }
        }

        private int BackingDictionaryValueCount => this.BackingDictionary.Values.Sum(v => v.Count);
        public Guid Guid { get; }

        public (string? section, string? option, IConfigurationValue? value) this[Guid valueGuid] 
            => this.ValueBackingDictionary.TryGetValue(valueGuid, out var value) ? value : (null, null, null);

        public IEnumerator<(string, string, IConfigurationValue)> GetEnumerator()
        {
            return (from kvp in this.BackingDictionary
                from valuekvp in kvp.Value
                select (kvp.Key, valuekvp.Key, valuekvp.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
