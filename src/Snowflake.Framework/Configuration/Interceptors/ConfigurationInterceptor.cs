using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Snowflake.Configuration;

namespace Snowflake.Configuration.Interceptors
{
    public class ConfigurationInterceptor : IInterceptor
    {
        internal IConfigurationValueCollection Values { get; }
        private IConfigurationSectionDescriptor Descriptor { get; }  

        //public ConfigurationInterceptor(IConfigurationSectionDescriptor descriptor)
        //{
        //    this.Values = descriptor.Options.ToDictionary(p => p.OptionKey, p => new ConfigurationValue(p.Default) as IConfigurationValue);
        //}

        public ConfigurationInterceptor(IConfigurationSectionDescriptor descriptor, IConfigurationValueCollection values)
        {
            this.Values = values;
            this.Descriptor = descriptor;
        }

        /// <inheritdoc/>
        public void Intercept(IInvocation invocation)
        {
            var propertyName = invocation.Method.Name.Substring(4); // remove get_ or set_
            if (!this.Values[this.Descriptor].ContainsKey(propertyName))
            {
                invocation.Proceed();
            }
            else
            {
                if (invocation.Method.Name.StartsWith("get_"))
                {
                    invocation.ReturnValue = this.Values[this.Descriptor, propertyName].Value;
                }

                if (invocation.Method.Name.StartsWith("set_"))
                {
                    this.Values[this.Descriptor, propertyName].Value = invocation.Arguments[0];
                }
            }
        }
    }
}
