using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Snowflake.Configuration;

namespace Snowflake.DynamicConfiguration.Interceptors
{
    public class ConfigurationInterceptor : IInterceptor
    {
        internal readonly IDictionary<string, IConfigurationValue> Values;

        public ConfigurationInterceptor(IConfigurationSectionDescriptor descriptor)
        {
            this.Values = descriptor.Options.ToDictionary(p => p.KeyName, p => new ConfigurationValue(p.Default) as IConfigurationValue);
        }

        public ConfigurationInterceptor(IConfigurationSectionDescriptor descriptor, IDictionary<string, IConfigurationValue> values)
        {
            this.Values = descriptor.Options.ToDictionary(p => p.KeyName,
                p => values.ContainsKey(p.KeyName) ? values[p.KeyName] : new ConfigurationValue(p.Default));
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
