using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Immutable;
using Snowflake.Configuration.Utility;
using System.Linq.Expressions;
using Snowflake.Configuration.Extensions;
using Snowflake.Configuration.Attributes;
using Castle.Core.Internal;
using System.Reflection;
using EnumsNET.NonGeneric;
using System.ComponentModel;

namespace Snowflake.Configuration
{
    public class ConfigurationValueCollection : IConfigurationValueCollection
    {
        private IDictionary<string, Dictionary<string, IConfigurationValue>> BackingDictionary { get; }
        public ConfigurationValueCollection() : this(Enumerable.Empty<(string, string, IConfigurationValue)>())
        {

        }

        public ConfigurationValueCollection(IEnumerable<(string, string, IConfigurationValue)> values)
            : this(values, Guid.NewGuid())
        {

        }

        private static object FromString(string strValue, Type optionType)
        {
            return optionType == typeof(string)
                ? strValue // return string value if string
                : optionType.GetTypeInfo().IsEnum
                    ? NonGenericEnums.Parse(optionType, strValue) // return parsed enum if enum
                    : TypeDescriptor.GetConverter(optionType).ConvertFromInvariantString(strValue);
        }

        public static IConfigurationValueCollection MakeExistingValueCollection<T>
               (IEnumerable<(string section, string option, (string stringValue, Guid guid) value)> values, 
                    Guid collectionGuid)
            where T : class, IConfigurationCollection, IConfigurationCollection<T>
        {
            var typedValues = new List<(string, string, IConfigurationValue)>();
            foreach (var section in from props in typeof(T).GetPublicProperties()
                                    let sectionAttr = props.GetAttributes<SerializableSectionAttribute>().FirstOrDefault()
                                    where sectionAttr != null
                                    select new
                                    {
                                        sectionAttr,
                                        type = props.PropertyType,
                                        name = props.Name
                                    })
            {
                var sectionDescType = typeof(ConfigurationSectionDescriptor<>).MakeGenericType(section.type);
                var descriptor = Instantiate.CreateInstance(sectionDescType,
                    new[] { typeof(string) },
                    Expression.Constant(section.name)) as IConfigurationSectionDescriptor;
                foreach (var tuple in values.Where(s => s.section == descriptor.SectionKey)) {
                    Type? t = descriptor?[tuple.option]?.Type;
                    typedValues.Add((tuple.section, tuple.option, 
                        new ConfigurationValue(FromString(tuple.value.stringValue, t), tuple.value.guid)));
                }
            }
            return new ConfigurationValueCollection(typedValues, collectionGuid);
        }

        public static IConfigurationValueCollection MakeExistingValueCollection<T>
              (IEnumerable<(string option, (string stringValue, Guid guid) value)> values,
                   string sectionName,
                   Guid collectionGuid)
           where T : class, IConfigurationSection<T>
        {
            var typedValues = new List<(string, string, IConfigurationValue)>();

            var sectionDescType = typeof(ConfigurationSectionDescriptor<>).MakeGenericType(typeof(T));
            var descriptor = Instantiate.CreateInstance(sectionDescType,
                new[] { typeof(string) },
                Expression.Constant(sectionName)) as IConfigurationSectionDescriptor;
            foreach (var tuple in values)
            {
                Type? t = descriptor?[tuple.option]?.Type;
                typedValues.Add((sectionName, tuple.option,
                    new ConfigurationValue(FromString(tuple.value.stringValue, t), tuple.value.guid)));
            }

            return new ConfigurationValueCollection(typedValues, collectionGuid);
        }


        internal ConfigurationValueCollection(IEnumerable<(string section, string option, IConfigurationValue value)> values, Guid guid)
        {
            this.ValueCollectionGuid = guid;
            var defs = values.GroupBy(p => p.section)
                .ToDictionary(p => p.Key, p => p.ToDictionary(o => o.option, k => k.value));
            this.BackingDictionary = defs;
        }

        public IReadOnlyDictionary<string, IConfigurationValue> this[IConfigurationSectionDescriptor descriptor] {
            get
            {
                this.EnsureSectionDefaults(descriptor);
                return ImmutableDictionary.CreateRange(this.BackingDictionary[descriptor.SectionKey]);
            }
        }

        internal void EnsureSectionDefaults(IConfigurationSectionDescriptor descriptor)
        {
            if (!this.BackingDictionary.ContainsKey(descriptor.SectionKey))
            {
                this.BackingDictionary[descriptor.SectionKey] = new Dictionary<string, IConfigurationValue>();
            }

            foreach (var prop in descriptor.Options)
            {
                if (!this.BackingDictionary[descriptor.SectionKey].ContainsKey(prop.OptionKey))
                {
                    this.BackingDictionary[descriptor.SectionKey][prop.OptionKey] = new ConfigurationValue(prop.Default);
                }
            }

        }

        public IConfigurationValue? this[IConfigurationSectionDescriptor descriptor, string option]
        {
            get
            {
                this.EnsureSectionDefaults(descriptor);
                if (!this.BackingDictionary[descriptor.SectionKey].ContainsKey(option)) return null;
                return this.BackingDictionary[descriptor.SectionKey][option];
            }
        }

        public Guid ValueCollectionGuid { get; }

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
