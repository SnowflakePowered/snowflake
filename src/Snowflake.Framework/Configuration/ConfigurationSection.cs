using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Snowflake.Configuration.Generators;
using Snowflake.Configuration.Utility;

namespace Snowflake.Configuration
{
    public class ConfigurationSection<T> : IConfigurationSection<T>
        where T : class
    {
        /// <inheritdoc/>
        public T Configuration { get; }

        /// <inheritdoc/>
        public IConfigurationSectionDescriptor Descriptor { get; }

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, IConfigurationValue> Values
            => ImmutableDictionary.CreateRange(this.ValueCollection[this.Descriptor]);

        public IConfigurationValueCollection ValueCollection { get; }

        /// <inheritdoc/>
        public object? this[string key]
        {
            //todo: make work
            get => this.ValueCollection[this.Descriptor, key]?.Value; 
            set
            {
                this.ValueCollection[this.Descriptor, key]!.Value = value!;
            }
        }

        public ConfigurationSection()
            : this(new ConfigurationValueCollection(), typeof(T).Name)
        {
        }

        public ConfigurationSection(IConfigurationValueCollection values, 
            IConfigurationSectionDescriptor sectionDescriptor)
        {
            this.Descriptor = sectionDescriptor;
            // if this is a CVC base implementation, we should ensure defaults.
            (values as ConfigurationValueCollection)?.EnsureSectionDefaults(this.Descriptor);
            this.ValueCollection = values;
            var genInstance = typeof(T).GetCustomAttribute<ConfigurationGenerationInstanceAttribute>();
            if (genInstance == null)
                throw new InvalidOperationException("Not generated!"); // todo: mark with interface to fail at compile time.

            this.Configuration =
                (T)Instantiate.CreateInstance(genInstance.InstanceType,
                    new[] { typeof(IConfigurationSectionDescriptor), typeof(IConfigurationValueCollection) },
                    Expression.Constant(this.Descriptor), Expression.Constant(this.ValueCollection));
        }

        public ConfigurationSection(IConfigurationValueCollection values,
           IConfigurationSectionDescriptor sectionDescriptor, T configurationInstance)
        {
            this.Descriptor = sectionDescriptor;
            // if this is a CVC base implementation, we should ensure defaults.
            (values as ConfigurationValueCollection)?.EnsureSectionDefaults(this.Descriptor);
            this.ValueCollection = values;
            this.Configuration = configurationInstance;
        }

        public ConfigurationSection(IConfigurationValueCollection values, string sectionKey)
            : this(values, new ConfigurationSectionDescriptor<T>(sectionKey))
        {
            
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
