using System.Collections.Generic;
using Castle.DynamicProxy;
using Snowflake.Configuration;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Interceptors
{
    internal class InputTemplateInterceptor<T> : IInterceptor
    {
        internal InputTemplateInterceptor(IDictionary<string, ControllerElement> inputValues, IDictionary<string, IConfigurationValue> configValues)
        {
            this.InputValues = inputValues;
            this.ConfigValues = configValues;
        }

        internal readonly IDictionary<string, IConfigurationValue> ConfigValues;
        internal IDictionary<string, ControllerElement> InputValues;
        public void Intercept(IInvocation invocation)
        {
            var propertyName = invocation.Method.Name.Substring(4); // remove get_ or set_
            if (this.InputValues.ContainsKey(propertyName))
            {
                if (invocation.Method.Name.StartsWith("get_"))
                {
                    invocation.ReturnValue = InputValues[propertyName]; //type is IConfigurationSection<T>
                }
            }
            else if (this.ConfigValues.ContainsKey(propertyName))
            {
                if (invocation.Method.Name.StartsWith("get_"))
                {
                    invocation.ReturnValue = ConfigValues[propertyName].Value;
                }
                if (invocation.Method.Name.StartsWith("set_"))
                {
                    ConfigValues[propertyName].Value = invocation.Arguments[0];
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
    }

