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

        public IConfigurationValueCollection ValueCollection { get; }

        /// <inheritdoc/>
        public object? this[string key]
        {
            get { return configurationInterceptor.Values[this.Descriptor, key]?.Value; }
            set
            {
                this.configurationInterceptor.Values[this.Descriptor, key]!.Value = value!;
            }
        }

        private readonly ConfigurationInterceptor configurationInterceptor;

        internal ConfigurationSection()
            : this(new ConfigurationValueCollection(), typeof(T).Name)
        {
        }

        internal ConfigurationSection(IConfigurationValueCollection values, string sectionKey)
        {
            this.Descriptor = new ConfigurationSectionDescriptor<T>(sectionKey);
            this.configurationInterceptor = new ConfigurationInterceptor(this.Descriptor, values);
            // if this is a CVC base implementation, we should ensure defaults.
            (values as ConfigurationValueCollection)?.EnsureSectionDefaults(this.Descriptor);
            this.ValueCollection = values;

            this.Configuration =
                ConfigurationDescriptorCache
                    .GetProxyGenerator()
                    .CreateInterfaceProxyWithoutTarget<T>(new ConfigurationCircularInterceptor<T>(this),
                        configurationInterceptor);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<IConfigurationOptionDescriptor, IConfigurationValue>> GetEnumerator()
        {
            return this.Descriptor.Options
                .Select(o =>
                    new KeyValuePair<IConfigurationOptionDescriptor, IConfigurationValue>(o, this.Values[o.OptionKey]))
                .GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
