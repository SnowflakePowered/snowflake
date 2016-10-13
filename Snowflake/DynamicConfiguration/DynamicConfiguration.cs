using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Snowflake.Configuration.Attributes;

namespace Snowflake.DynamicConfiguration
{
    public class DynamicConfiguration<T> where T : class
    {
        public T Configuration { get; }
        public IList<ConfigurationOption> Options { get; }
        public DynamicConfiguration()
        {
            ProxyGenerator generator = new ProxyGenerator();
            var options = typeof(T).GetProperties().Select(prop =>
                        new {attr = prop.GetCustomAttributes<ConfigurationOptionAttribute>().First(), name = prop.Name, metadata = prop.GetCustomAttributes<CustomMetadataAttribute>()})
                .Select(attr => new ConfigurationOption(attr.attr, attr.metadata, attr.name));
            this.Options = options.ToList();
            var interceptor = new ConfigurationInterceptor(this.Options.ToDictionary(p => p, p => p.Default));
            this.Configuration = generator.CreateInterfaceProxyWithoutTarget<T>(interceptor);
        }
    }

    internal class ConfigurationInterceptor : IInterceptor
    {
        private readonly IDictionary<string, ConfigurationValue> Values;

        public ConfigurationInterceptor(IDictionary<ConfigurationOption, object> options)
        {
            this.Values = options.ToDictionary(p => p.Key.KeyName, p => new ConfigurationValue(p.Key, p.Value));
        }

        public ConfigurationInterceptor(IDictionary<string, object> values, IList<ConfigurationOption> options)
        {
            this.Values = options.ToDictionary(p => p.KeyName, p 
                => new ConfigurationValue(p, values.ContainsKey(p.KeyName) ? values[p.KeyName] : p.Default));
        }
        public void Intercept(IInvocation invocation)
        {
            var propertyName = invocation.Method.Name.Substring(4); // remove get_ or set_
            if (!this.Values.ContainsKey(propertyName)) return;
            if (invocation.Method.Name.StartsWith("get_"))
            {
                invocation.ReturnValue = Values[propertyName].Value;
            }
            if (invocation.Method.Name.StartsWith("set_"))
            {
                Values[propertyName].Value = invocation.Arguments[0];
            }
        }
    }
}
