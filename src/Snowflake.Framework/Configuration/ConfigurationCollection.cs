using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Snowflake.Configuration.Generators;
using Snowflake.Configuration.Utility;

namespace Snowflake.Configuration
{
    public class ConfigurationCollection<T> : IConfigurationCollection<T>
        where T : class, IConfigurationCollectionTemplate
    {
        /// <inheritdoc/>
        public T Configuration { get; }

        /// <inheritdoc/>
        public IConfigurationCollectionDescriptor Descriptor { get; }

        public IConfigurationValueCollection ValueCollection { get; }

        public ConfigurationCollection()
            : this(new ConfigurationValueCollection())
        {
        }

        internal ConfigurationCollection(IConfigurationValueCollection defaults)
        {
            this.Descriptor =
                ConfigurationDescriptorCache.GetCollectionDescriptor<T>();

            var genInstance = typeof(T).GetCustomAttribute<ConfigurationGenerationInstanceAttribute>();
            if (genInstance == null)
                throw new InvalidOperationException("Not generated!"); // todo: mark with interface to fail at compile time.
            this.ValueCollection = defaults;

            this.Configuration = (T)Instantiate.CreateInstance(genInstance.InstanceType,
                    new[] { typeof(IConfigurationValueCollection) }, Expression.Constant(this.ValueCollection));
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, IConfigurationSection>> GetEnumerator()
        {
            var values = this.Configuration.GetValueDictionary();

            return this.Descriptor
                .SectionKeys.Select(k => new KeyValuePair<string, IConfigurationSection?>(
                    k, values[k]))
                .GetEnumerator() ?? 
                Enumerable.Empty<KeyValuePair<string, IConfigurationSection>>().GetEnumerator(); 
            // re-iterate to ensure order
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IConfigurationSection<TSection> GetSection<TSection>(Expression<Func<T, TSection>> expression) where TSection : class
        {
            if (expression.Body is not MemberExpression member)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                     expression.ToString()));

            if (this.GetSection(member.Member.Name) as IConfigurationSection<TSection> is IConfigurationSection<TSection> section)
            {
                return section;
            }
            throw new InvalidOperationException($"Unable to find section {member.Member.Name}. Something went horribly wrong.");
        }

        /// <inheritdoc/>
        public IConfigurationSection? GetSection(string sectionName)
        {
            var values = this.Configuration.GetValueDictionary();
            if (values != null && values.TryGetValue(sectionName, out var section))
            {
                return section;
            }
            return null;
        }
    }
}
