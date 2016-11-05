using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using NLog.LayoutRenderers.Wrappers;
using Snowflake.Configuration.Attributes;
using Snowflake.DynamicConfiguration.Interceptors;

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

        public ConfigurationSection(IDictionary<string, IConfigurationValue> values)
        {
            ProxyGenerator generator = new ProxyGenerator();
            this.Descriptor = new ConfigurationSectionDescriptor<T>();
            this.configurationInterceptor = new ConfigurationInterceptor(this.Descriptor, values);
          
            this.Configuration =
                generator.CreateInterfaceProxyWithoutTarget<T>(new ConfigurationCircularInterceptor<T>(this),
                    configurationInterceptor);
        }

    
    }



}
