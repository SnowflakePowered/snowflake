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
using NLog.LayoutRenderers.Wrappers;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Interceptors;

namespace Snowflake.Configuration
{
    public class ConfigurationSection<T> : IConfigurationSection<T> where T : class, IConfigurationSection<T>
    {
        public T Configuration { get; }
        public IConfigurationSectionDescriptor Descriptor { get; }

        public IDictionary<string, IConfigurationValue> Values
            => ImmutableDictionary.CreateRange(this.configurationInterceptor.Values);

        public object this[string key]
        {
            get { return configurationInterceptor.Values[key]; }
            set { this.configurationInterceptor.Values[key].Value = value; }
        }

        private readonly ConfigurationInterceptor configurationInterceptor;

        public ConfigurationSection(): this(new Dictionary<string, IConfigurationValue>())
        {
        }

        internal ConfigurationSection(IDictionary<string, ValueTuple<string, Guid>> values)
        {
            ProxyGenerator generator = new ProxyGenerator();
            this.Descriptor = ConfigurationDescriptorCache.GetSectionDescriptor<T>();
            this.configurationInterceptor = new ConfigurationInterceptor(this.Descriptor,
                values.ToDictionary(p => p.Key, FromValueTuple));

            this.Configuration =
                generator.CreateInterfaceProxyWithoutTarget<T>(new ConfigurationCircularInterceptor<T>(this),
                    configurationInterceptor);
        }

        public ConfigurationSection(IDictionary<string, IConfigurationValue> values)
        {
            ProxyGenerator generator = new ProxyGenerator();
            this.Descriptor = new ConfigurationSectionDescriptor<T>();
            this.configurationInterceptor = new ConfigurationInterceptor(this.Descriptor, values);
          
            this.Configuration =
                generator.CreateInterfaceProxyWithoutTarget<T>(new ConfigurationCircularInterceptor<T>(this),
                    configurationInterceptor);
        }

        private static object FromString(string strValue, Type optionType)
        {
            return optionType == typeof(string)
                ? strValue //return string value if string
                : optionType.IsEnum
                    ? NonGenericEnums.Parse(optionType, strValue)//return parsed enum if enum
                    : TypeDescriptor.GetConverter(optionType).ConvertFromInvariantString(strValue);
        }
        private IConfigurationValue FromValueTuple(KeyValuePair<string, ValueTuple<string, Guid>> tuple)
        {
            Type t = this.Descriptor[tuple.Key].Type;
            return new ConfigurationValue(FromString(tuple.Value.Item1, t), tuple.Value.Item2);
        }

        public IEnumerator<KeyValuePair<IConfigurationOption, IConfigurationValue>> GetEnumerator()
        {
            return this.Descriptor.Options
                .Select(o => new KeyValuePair<IConfigurationOption, IConfigurationValue>(o, this.Values[o.KeyName]))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }



}
