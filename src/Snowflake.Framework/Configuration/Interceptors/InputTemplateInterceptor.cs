using System.Collections.Generic;
using Castle.DynamicProxy;
using Snowflake.Configuration;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Interceptors
{
    internal class InputTemplateInterceptor<T> : IInterceptor
    {
        internal InputTemplateInterceptor(
            IDictionary<string, ControllerElement> inputValues,
            IConfigurationValueCollection configValues,
            IConfigurationSectionDescriptor descriptor)
        {
            this.Descriptor = descriptor;
            this.InputValues = inputValues;
            this.ConfigValues = configValues;
        }

        internal IConfigurationValueCollection ConfigValues { get; }
        public IConfigurationSectionDescriptor Descriptor { get; }

        internal IDictionary<string, ControllerElement> InputValues;

        /// <inheritdoc/>
        public void Intercept(IInvocation invocation)
        {
            var propertyName = invocation.Method.Name.Substring(4); // remove get_ or set_
            if (this.InputValues.ContainsKey(propertyName))
            {
                if (invocation.Method.Name.StartsWith("get_"))
                {
                    invocation.ReturnValue = InputValues[propertyName]; // type is IConfigurationSection<T>
                }
            }
            else if (this.ConfigValues[this.Descriptor].ContainsKey(propertyName))
            {
                if (invocation.Method.Name.StartsWith("get_"))
                {
                    invocation.ReturnValue = this.ConfigValues[this.Descriptor, propertyName]!.Value;
                }

                if (invocation.Method.Name.StartsWith("set_"))
                {
                    this.ConfigValues[this.Descriptor, propertyName]!.Value = invocation.Arguments[0];
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
