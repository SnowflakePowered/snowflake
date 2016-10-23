using Castle.DynamicProxy;

namespace Snowflake.DynamicConfiguration.Interceptors
{

    /// <summary>
    /// Interceptor to allow circular reference within <see cref="ConfigurationSection{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ConfigurationCircularInterceptor<T> : IInterceptor where T : class, IConfigurationSection<T>
    {
        private readonly IConfigurationSection<T> @this;
        public ConfigurationCircularInterceptor(IConfigurationSection<T> @this)
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
                default:
                    invocation.Proceed();
                    break;
            }
        }
    }

}
