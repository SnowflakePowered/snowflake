using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.Utility;

namespace Snowflake.Configuration
{
    public class ConfigurationCollection<T> : IConfigurationCollection<T> where T: class, IConfigurationCollection<T>
    {
        public T Configuration { get; }
        public IConfigurationCollectionDescriptor Descriptor { get; }
        public IDictionary<string, IConfigurationSection> Sections
            => this.collectionInterceptor.Values.ToDictionary(p => p.Key, p => p.Value as IConfigurationSection);
        private readonly CollectionInterceptor<T> collectionInterceptor;

        public ConfigurationCollection() : this(new Dictionary<string, IDictionary<string, IConfigurationValue>>())
        {
        }

        public ConfigurationCollection(IDictionary<string, IDictionary<string, IConfigurationValue>> defaults)
        {
            ProxyGenerator generator = new ProxyGenerator();
            this.Descriptor = new ConfigurationCollectionDescriptor<T>();
            this.collectionInterceptor = new CollectionInterceptor<T>(defaults);
            this.Configuration = generator.CreateInterfaceProxyWithoutTarget<T>(new CollectionCircularInterceptor<T>(this), this.collectionInterceptor);
        }

        public IEnumerator<IConfigurationSection> GetEnumerator() => this.collectionInterceptor.Values.Values.Select(c => c as IConfigurationSection).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IDictionary<string, string> Outputs { get; }

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
                    case nameof(@this.Outputs):
                        invocation.ReturnValue = @this.Outputs;
                        break;
                    case nameof(@this.Sections):
                        invocation.ReturnValue = @this.Sections;
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

        
        internal CollectionInterceptor(IDictionary<string, IDictionary<string, IConfigurationValue>> defaults)
        {
            this.Values = new Dictionary<string, dynamic>();
            //public ConfigurationSection(IDictionary<string, IConfigurationValue> values)
            foreach (var section in from props in typeof(T).GetProperties()
                                    let sectionAttr = props.GetAttributes<SerializableSectionAttribute>().First()
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
