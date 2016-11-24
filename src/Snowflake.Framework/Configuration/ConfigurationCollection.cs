using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.JsonConverters;
using Snowflake.Utility;

namespace Snowflake.Configuration
{
    [JsonConverter(typeof(ConfigurationCollectionSerializer))]
    public class ConfigurationCollection<T> : IConfigurationCollection<T> where T: class, IConfigurationCollection<T>
    {
        public T Configuration { get; }
        public IConfigurationCollectionDescriptor Descriptor { get; }

        private readonly CollectionInterceptor<T> collectionInterceptor;

        public ConfigurationCollection() : this(new Dictionary<string, IDictionary<string, IConfigurationValue>>())
        {
        }

        public ConfigurationCollection(IDictionary<string, IDictionary<string, IConfigurationValue>> defaults)
        {
            ProxyGenerator generator = new ProxyGenerator();
            this.Descriptor =
                ConfigurationDescriptorCache.GetCollectionDescriptor<T>();
            this.collectionInterceptor = new CollectionInterceptor<T>(defaults);
            this.Configuration = generator.CreateInterfaceProxyWithoutTarget<T>(new CollectionCircularInterceptor<T>(this), this.collectionInterceptor);
        }
        /// <summary>
        /// Used for the sqlite configuration collection store
        /// </summary>
        /// <param name="defaults"></param>
        internal ConfigurationCollection(IDictionary<string, IDictionary<string, ValueTuple<string, Guid>>> defaults)
        {
            ProxyGenerator generator = new ProxyGenerator();
            this.Descriptor =
                ConfigurationDescriptorCache.GetCollectionDescriptor<T>();
            this.collectionInterceptor = new CollectionInterceptor<T>(defaults);
            this.Configuration = generator.CreateInterfaceProxyWithoutTarget<T>(new CollectionCircularInterceptor<T>(this), this.collectionInterceptor);
        }
        public IEnumerator<KeyValuePair<string, IConfigurationSection>> GetEnumerator()
        {
            return this.Descriptor.SectionKeys.Select(k => new KeyValuePair<string, IConfigurationSection>
            (k, this.collectionInterceptor.Values[k] as IConfigurationSection))
                .GetEnumerator(); //ensure order
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IConfigurationSection this[string sectionName] => this.collectionInterceptor.Values[sectionName];
    }

    class CollectionCircularInterceptor<T> : IInterceptor where T : class, IConfigurationCollection<T>
    {
        private readonly IConfigurationCollection<T> @this;
        public CollectionCircularInterceptor(IConfigurationCollection<T> @this)
        {
            this.@this = @this;
        }
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name == nameof(@this.GetEnumerator))
            {
                invocation.ReturnValue = @this.GetEnumerator(); //inherit enumerator.
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
                    case "Item": //circular indexer
                        invocation.ReturnValue = @this[(string) invocation.Arguments[0]];
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
        internal readonly IDictionary<string, dynamic> Values;

        internal CollectionInterceptor(IDictionary<string, IDictionary<string, ValueTuple<string, Guid>>> defaults)
        {
            this.Values = new Dictionary<string, dynamic>();
            foreach (var section in from props in typeof(T).GetPublicProperties()
                                    let sectionAttr = props.GetAttributes<SerializableSectionAttribute>().FirstOrDefault()
                                    where sectionAttr != null
                                    select new { sectionAttr, type = props.PropertyType, name = props.Name })
            {
                var sectionType = typeof(ConfigurationSection<>).MakeGenericType(section.type);
                this.Values.Add(section.name, Instantiate.CreateInstance(sectionType, new[] { typeof(IDictionary<string, ValueTuple<string, Guid>>) },
                    Expression.Constant(defaults.ContainsKey(section.name) ? defaults[section.name] 
                    : new Dictionary<string, ValueTuple<string, Guid>>())));
            }
        }

        internal CollectionInterceptor(IDictionary<string, IDictionary<string, IConfigurationValue>> defaults)
        {
            this.Values = new Dictionary<string, dynamic>();
            //public ConfigurationSection(IDictionary<string, IConfigurationValue> values)
            foreach (var section in from props in typeof(T).GetPublicProperties()
                                    let sectionAttr = props.GetAttributes<SerializableSectionAttribute>().FirstOrDefault()
                                    where sectionAttr !=null
                                    select new {sectionAttr, type = props.PropertyType, name = props.Name})
            {
                var sectionType = typeof(ConfigurationSection<>).MakeGenericType(section.type);
                this.Values.Add(section.name, Instantiate.CreateInstance(sectionType, new Type[] {typeof(IDictionary<string, IConfigurationValue>) },
                    Expression.Constant(defaults.ContainsKey(section.name)? defaults[section.name] : new Dictionary<string, IConfigurationValue>())));
            }
        }


        public void Intercept(IInvocation invocation)
        {
            var propertyName = invocation.Method.Name.Substring(4); // remove get_ or set_
            if (!this.Values.ContainsKey(propertyName)) return;
            if (invocation.Method.Name.StartsWith("get_"))
            {
                invocation.ReturnValue = Values[propertyName].Configuration; //type is IConfigurationSection<T>
            }
        }
    }
}
