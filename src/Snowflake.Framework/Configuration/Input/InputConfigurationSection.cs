using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Interceptors;

namespace Snowflake.Configuration.Input
{
    public class InputConfigurationSection<T> : IConfigurationSection<T>
        where T : class, IInputTemplate<T>
    {
        /// <inheritdoc/>
        public T Configuration { get; }

        /// <inheritdoc/>
        public IConfigurationSectionDescriptor Descriptor { get; }

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, IConfigurationValue> Values
            => ImmutableDictionary.CreateRange(this.configurationInterceptor.Values[this.Descriptor]);

        public IConfigurationValueCollection ValueCollection { get; }

        /// <inheritdoc/>
        public object? this[string key]
        {
            get => configurationInterceptor.Values[this.Descriptor, key];
            set
            {
                var val = this.configurationInterceptor.Values[this.Descriptor, key];
                if (val != null)
                {
                    val.Value = value!;
                }
            }
        }

        private readonly ConfigurationInterceptor configurationInterceptor;

        internal InputConfigurationSection(InputTemplateCircularInterceptor<T> interceptor,
            InputTemplateInterceptor<T> inputTemplate)
        {
            this.Descriptor = new ConfigurationSectionDescriptor<T>(typeof(T).Name);
            ProxyGenerator generator = new ProxyGenerator();
 
            // todo: fix this.
            this.ValueCollection = new ConfigurationValueCollection();
            this.configurationInterceptor = new ConfigurationInterceptor(this.Descriptor, this.ValueCollection);
            this.Configuration =
                generator.CreateInterfaceProxyWithoutTarget<T>(interceptor, configurationInterceptor, inputTemplate);
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
