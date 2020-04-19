using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Snowflake.Loader;

namespace Snowflake.Remoting.Kestrel
{
    /// <summary>
    /// Provides the implementation for a Kestrel middleware.
    /// <para>
    /// Since Kestrel is bootstraped by ASP.NET Core, it uses it's own dependency injection container 
    /// (namely <see cref="IServiceCollection"/>), instead of Snowflake's native <see cref="Snowflake.Services.IServiceContainer"/>.
    /// Implementing this interface is akin to implementing <see cref="IComposable"/>, but specifically to extend the integrated Kestrel 
    /// web server.
    /// </para>
    /// </summary>
    public interface IKestrelServerMiddlewareProvider
    {
        /// <summary>
        /// Use this method to add services to the container that is used by the Kestrel server.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> used by the Kestrel server.</param>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// Use this method to configure the HTTP request pipeline in Kestrel.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> used by the Kestrel server.</param>
        void Configure(IApplicationBuilder app);
    }
}
