using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Extensions;
using Snowflake.Configuration.Utility;
using Snowflake.JsonConverters;

namespace Snowflake.Configuration
{
    [JsonConverter(typeof(ConfigurationCollectionSerializer))]
    public class ConfigurationCollection<T> : IConfigurationCollection<T>
        where T : class, IConfigurationCollection<T>
    {
        /// <inheritdoc/>
        public T Configuration { get; }

        
        /// <inheritdoc/>
        public IConfigurationCollectionDescriptor Descriptor { get; }

        public IConfigurationValueCollection ValueCollection { get; }

        private readonly CollectionInterceptor<T> collectionInterceptor;

        public ConfigurationCollection()
            : this(new ConfigurationValueCollection())
        {
        }

        internal ConfigurationCollection(IConfigurationValueCollection defaults)
        {

            this.Descriptor =
                ConfigurationDescriptorCache.GetCollectionDescriptor<T>();
            this.collectionInterceptor = new CollectionInterceptor<T>(defaults);

            this.Configuration = ConfigurationDescriptorCache
                    .GetProxyGenerator().CreateInterfaceProxyWithoutTarget<T>
                    (new CollectionCircularInterceptor<T>(this), this.collectionInterceptor);

            this.ValueCollection = defaults;
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, IConfigurationSection>> GetEnumerator()
        {
            return this.Descriptor.SectionKeys.Select(k => new KeyValuePair<string, IConfigurationSection>(
            k, this.collectionInterceptor.Values[k] as IConfigurationSection))
                .GetEnumerator(); // ensure order
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc/>
        public IConfigurationSection this[string sectionName] => this.collectionInterceptor.Values[sectionName];
    }

    class CollectionCircularInterceptor<T> : IInterceptor
        where T : class, IConfigurationCollection<T>
    {
        private readonly IConfigurationCollection<T> @this;
        public CollectionCircularInterceptor(IConfigurationCollection<T> @this)
        {
            this.@this = @this;
        }

        /// <inheritdoc/>
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name == nameof(@this.GetEnumerator))
            {
                invocation.ReturnValue = @this.GetEnumerator(); // inherit enumerator.
            }
            else
            {
                switch (invocation.Method.Name.Substring(4))
                {
                    case nameof(@this.Configuration):
                        invocation.ReturnValue = @this.Configuration;
                        break;
                    case nameof(@this.Descriptor):
                        invocation.ReturnValue = @this.Descriptor;
                        break;
                    case nameof(@this.ValueCollection):
                        invocation.ReturnValue = @this.ValueCollection;
                        break;
                    case "Item": // circular indexer
                        invocation.ReturnValue = @this[(string)invocation.Arguments[0]];
                        break;
                    default:
                        invocation.Proceed();
                        break;
                }
            }
        }
    }

    class CollectionInterceptor<T> : IInterceptor
    {
        // IDictionary<string, IConfigurationSection<T>>
        internal IDictionary<string, dynamic> Values;

        internal CollectionInterceptor(IEnumerable<(string section, string key, (string, Guid) strvalue)> values)
        {
            this.Values = new Dictionary<string, dynamic>();
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
                    Expression.Constant(section.name));
            }
        }

        internal CollectionInterceptor(IConfigurationValueCollection defaults)
        {
            this.Values = new Dictionary<string, dynamic>();

            // public ConfigurationSection(IDictionary<string, IConfigurationValue> values)
            foreach (var section in from props in typeof(T).GetPublicProperties()
                                    let sectionAttr = props.GetAttributes<SerializableSectionAttribute>().FirstOrDefault()
                                    where sectionAttr != null
                                    select new { sectionAttr, type = props.PropertyType, name = props.Name })
            {
                var sectionType = typeof(ConfigurationSection<>).MakeGenericType(section.type);

                if (section?.name != null)
                {
                    this.Values.Add(section.name,
                        Instantiate.CreateInstance(sectionType, new Type[] { typeof(IConfigurationValueCollection), typeof(string) },
                        Expression.Constant(defaults), Expression.Constant(section.name)));
                }
            }
        }

        /// <inheritdoc/>
        public void Intercept(IInvocation invocation)
        {
            var propertyName = invocation.Method.Name.Substring(4); // remove get_ or set_
            if (!this.Values.ContainsKey(propertyName))
            {
                return;
            }

            if (invocation.Method.Name.StartsWith("get_"))
            {
                invocation.ReturnValue = Values[propertyName].Configuration; // type is IConfigurationSection<T>
            }
        }
    }
}
