using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Services
{
    /// <summary>
    /// Provides privileged access to the service container.
    /// 
    /// Used only in Snowflake.Support.GraphQL.Server
    /// </summary>
    internal interface IPrivilegedServiceContainerAccessor
    {
        /// <summary>
        /// Get the currently loaded <see cref="IServiceContainer"/>.
        /// </summary>
        /// <returns>The service container.</returns>
        IServiceContainer GetServiceContainer();

        /// <summary>
        /// Get the currently loaded service container as an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <returns>The service container as an <see cref="IServiceProvider"/>.</returns>
        IServiceProvider GetServiceContainerAsServiceProvider();
    }
}
