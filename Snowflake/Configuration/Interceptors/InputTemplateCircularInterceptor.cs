using Castle.DynamicProxy;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;

namespace Snowflake.DynamicConfiguration.Interceptors
{

    internal class InputTemplateCircularInterceptor<T> : IInterceptor where T : class, IInputTemplate<T>
    {
        private readonly IInputTemplate<T> @this;

        public InputTemplateCircularInterceptor(IInputTemplate<T> @this)
        {
            this.@this = @this;
        }

        public void Intercept(IInvocation invocation)
        {
            switch (invocation.Method.Name.Substring(4))
            {
                case nameof(@this.Configuration):
                    invocation.ReturnValue = @this.Configuration;
                    break;
                case nameof(@this.Destination):
                    invocation.ReturnValue = @this.Destination;
                    break;
                case nameof(@this.Options):
                    invocation.ReturnValue = @this.Options;
                    break;
                case nameof(@this.Values):
                    invocation.ReturnValue = @this.Values;
                    break;
                case nameof(@this.DisplayName):
                    invocation.ReturnValue = @this.DisplayName;
                    break;
                case nameof(@this.SectionName):
                    invocation.ReturnValue = @this.SectionName;
                    break;
                case nameof(@this.Description):
                    invocation.ReturnValue = @this.Description;
                    break;
                case nameof(@this.PlayerIndex):
                    invocation.ReturnValue = @this.PlayerIndex;
                    break;
                case nameof(@this.Template):
                    invocation.ReturnValue = @this.Template;
                    break;
                case "Item":
                    if (invocation.Method.DeclaringType == typeof(IConfigurationSection))
                    {
                        if (invocation.Method.Name.StartsWith("set_"))
                        {
                            @this[(string)invocation.Arguments[0]] = invocation.Arguments[1];
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
                            @this[(ControllerElement) invocation.Arguments[0]] = (ControllerElement)invocation.Arguments[1];
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
