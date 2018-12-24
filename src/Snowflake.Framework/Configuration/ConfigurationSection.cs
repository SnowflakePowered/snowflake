using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using EnumsNET.NonGeneric;
using Snowflake.Configuration.Interceptors;

namespace Snowflake.Configuration
{
    public class ConfigurationSection<T> : IConfigurationSection<T>
        where T : class, IConfigurationSection<T>
    {
        /// <inheritdoc/>
        public T Configuration { get; }

        /// <inheritdoc/>
        public IConfigurationSectionDescriptor Descriptor { get; }

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, IConfigurationValue> Values
            => ImmutableDictionary.CreateRange(this.configurationInterceptor.Values[this.Descriptor]);

        /// <inheritdoc/>
        public object? this[string key]
        {
            get { return configurationInterceptor.Values[this.Descriptor, key]?.Value; }
            set { this.configurationInterceptor.Values[this.Descriptor, key]!.Value = value!; }
        }

        private readonly ConfigurationInterceptor configurationInterceptor;

        internal ConfigurationSection()
            : this(new ConfigurationValueCollection(), typeof(T).Name)
        {
        }

        public ConfigurationSection(IConfigurationValueCollection values, string sectionKey)
        {
            this.Descriptor = new ConfigurationSectionDescriptor<T>(sectionKey);
            this.configurationInterceptor = new ConfigurationInterceptor(this.Descriptor, values);

            this.Configuration =
                  ConfigurationDescriptorCache
                .GetProxyGenerator().CreateInterfaceProxyWithoutTarget<T>(new ConfigurationCircularInterceptor<T>(this),
                    configurationInterceptor);
        }

        private static object FromString(string strValue, Type optionType)
        {
            return optionType == typeof(string)
                ? strValue // return string value if string
                : optionType.GetTypeInfo().IsEnum
                    ? NonGenericEnums.Parse(optionType, strValue) // return parsed enum if enum
                    : TypeDescriptor.GetConverter(optionType).ConvertFromInvariantString(strValue);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<IConfigurationOptionDescriptor, IConfigurationValue>> GetEnumerator()
        {
            return this.Descriptor.Options
                .Select(o => new KeyValuePair<IConfigurationOptionDescriptor, IConfigurationValue>(o, this.Values[o.OptionKey]))
                .GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
