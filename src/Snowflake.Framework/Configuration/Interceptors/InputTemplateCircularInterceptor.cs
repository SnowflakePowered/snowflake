using Castle.DynamicProxy;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;

namespace Snowflake.Configuration.Interceptors
{
    internal class InputTemplateCircularInterceptor<T> : IInterceptor
        where T : class, IInputTemplate<T>
    {
        private readonly IInputTemplate<T> @this;

        public InputTemplateCircularInterceptor(IInputTemplate<T> @this)
        {
            this.@this = @this;
        }

        /// <inheritdoc/>
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name == nameof(@this.GetEnumerator))
            {
                invocation.ReturnValue = @this.GetEnumerator();
                return;
            }

            switch (invocation.Method.Name.Substring(4))
            {
                case nameof(@this.Configuration):
                    invocation.ReturnValue = @this.Configuration;
                    break;
                case nameof(@this.Options):
                    invocation.ReturnValue = @this.Options;
                    break;
                case nameof(@this.Values):
                    invocation.ReturnValue = @this.Values;
                    break;
                case nameof(@this.Descriptor):
                    invocation.ReturnValue = @this.Descriptor;
                    break;
                case nameof(@this.PlayerIndex):
                    invocation.ReturnValue = @this.PlayerIndex;
                    break;
                case nameof(@this.ValueCollection):
                    invocation.ReturnValue = @this.ValueCollection;
                    break;
                case nameof(@this.Template):
                    invocation.ReturnValue = @this.Template;
                    break;
                case "Item":
                    if (invocation.Method.DeclaringType == typeof(IConfigurationSection))
                    {
                        if (invocation.Method.Name.StartsWith("set_"))
                        {
                            @this[(string) invocation.Arguments[0]] = invocation.Arguments[1];
                        }

                        if (invocation.Method.Name.StartsWith("get_"))
                        {
                            invocation.ReturnValue = @this[(string) invocation.Arguments[0]];
                        }

                        break;
                    }

                    if (invocation.Method.DeclaringType == typeof(IInputTemplate<T>))
                    {
                        if (invocation.Method.Name.StartsWith("set_"))
                        {
                            @this[(ControllerElement) invocation.Arguments[0]] =
                                (DeviceCapability) invocation.Arguments[1];
                        }
                    }

                    break;
                default:
                    invocation.Proceed();
                    break;
            }
        }
    }
}
