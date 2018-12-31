using Castle.DynamicProxy;
using Snowflake.Configuration;

namespace Snowflake.Configuration.Interceptors
{
    /// <summary>
    /// Interceptor to allow circular reference within <see cref="ConfigurationSection{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ConfigurationCircularInterceptor<T> : IInterceptor
        where T : class, IConfigurationSection<T>
    {
        private readonly IConfigurationSection<T> @this;
        public ConfigurationCircularInterceptor(IConfigurationSection<T> @this)
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
                case nameof(@this.Descriptor):
                    invocation.ReturnValue = @this.Descriptor;
                    break;
                case nameof(@this.Values):
                    invocation.ReturnValue = @this.Values;
                    break;
                case nameof(@this.ValueCollection):
                    invocation.ReturnValue = @this.ValueCollection;
                    break;
                default:
                    invocation.Proceed();
                    break;
            }
        }
    }
}
