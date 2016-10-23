using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace Snowflake.DynamicConfiguration.Interceptors
{
    public class ConfigurationInterceptor : IInterceptor
    {
        internal readonly IDictionary<string, IConfigurationValue> Values;

        public ConfigurationInterceptor(IDictionary<IConfigurationOption, object> options)
        {
            this.Values = options.ToDictionary(p => p.Key.KeyName, p => new ConfigurationValue(p.Key, p.Value) as IConfigurationValue);
        }

        public ConfigurationInterceptor(IDictionary<string, object> values, IList<IConfigurationOption> options)
        {
            this.Values = options.ToDictionary(p => p.KeyName, p
                => new ConfigurationValue(p, values.ContainsKey(p.KeyName) ? values[p.KeyName] : p.Default) as IConfigurationValue);
        }
        public void Intercept(IInvocation invocation)
        {
            var propertyName = invocation.Method.Name.Substring(4); // remove get_ or set_
            if (!this.Values.ContainsKey(propertyName))
            {
                invocation.Proceed();
            }
            else
            {
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
}
