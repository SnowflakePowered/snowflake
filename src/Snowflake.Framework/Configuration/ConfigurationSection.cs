using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using EnumsNET.NonGeneric;
using Snowflake.Configuration.Attributes;
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
        public IDictionary<string, IConfigurationValue> Values
            => ImmutableDictionary.CreateRange(this.configurationInterceptor.Values);

        /// <inheritdoc/>
        public object this[string key]
        {
            get { return configurationInterceptor.Values[key].Value; }
            set { this.configurationInterceptor.Values[key].Value = value; }
        }

        private readonly ConfigurationInterceptor configurationInterceptor;

        public ConfigurationSection()
            : this(new Dictionary<string, IConfigurationValue>())
        {
        }

        internal ConfigurationSection(IDictionary<string, (string stringValue, Guid guid)> values)
        {
            this.Descriptor = ConfigurationDescriptorCache.GetSectionDescriptor<T>();
            this.configurationInterceptor = new ConfigurationInterceptor(this.Descriptor,
                values.ToDictionary(p => p.Key, this.FromValueTuple));

            this.Configuration =
                ConfigurationDescriptorCache
                .GetProxyGenerator().CreateInterfaceProxyWithoutTarget<T>(new ConfigurationCircularInterceptor<T>(this),
                    this.configurationInterceptor);
        }

        public ConfigurationSection(IDictionary<string, IConfigurationValue> values)
        {
            this.Descriptor = new ConfigurationSectionDescriptor<T>();
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

        private IConfigurationValue FromValueTuple(KeyValuePair<string, (string stringValue, Guid guid)> tuple)
        {
            Type t = this.Descriptor[tuple.Key].Type;
            return new ConfigurationValue(FromString(tuple.Value.stringValue, t), tuple.Value.guid);
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
