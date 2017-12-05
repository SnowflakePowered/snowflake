using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Interceptors;
using Snowflake.Utility;

namespace Snowflake.Configuration.Input
{
    public class InputConfigurationSection<T> : IConfigurationSection<T>
        where T : class, IInputTemplate<T>
    {
        /// <inheritdoc/>
        public T Configuration { get; }
        public IEnumerable<IConfigurationOptionDescriptor> Options { get; }

        /// <inheritdoc/>
        public IConfigurationSectionDescriptor Descriptor { get; }

        /// <inheritdoc/>
        public IDictionary<string, IConfigurationValue> Values
            => ImmutableDictionary.CreateRange(this.configurationInterceptor.Values);

        /// <inheritdoc/>
        public object this[string key]
        {
            get { return configurationInterceptor.Values[key]; }
            set { this.configurationInterceptor.Values[key].Value = value; }
        }

        private readonly ConfigurationInterceptor configurationInterceptor;

        internal InputConfigurationSection(InputTemplateCircularInterceptor<T> interceptor, InputTemplateInterceptor<T> inputTemplate)
        {
            this.Descriptor = new ConfigurationSectionDescriptor<T>();
            ProxyGenerator generator = new ProxyGenerator();
            var options = from prop in typeof(T).GetProperties()
                          where prop.HasAttribute<ConfigurationOptionAttribute>()
                          let attr = prop.GetCustomAttribute<ConfigurationOptionAttribute>()
                          let name = prop.Name
                          let metadata = prop.GetCustomAttributes<CustomMetadataAttribute>()
                          select new ConfigurationOptionDescriptor(attr, metadata, name) as IConfigurationOptionDescriptor;

            this.Options = options.ToList();
            this.configurationInterceptor = new ConfigurationInterceptor(this.Descriptor);
            this.Configuration =
                generator.CreateInterfaceProxyWithoutTarget<T>(interceptor,
                    configurationInterceptor, inputTemplate);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<IConfigurationOptionDescriptor, IConfigurationValue>> GetEnumerator()
        {
            return this.Configuration.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
