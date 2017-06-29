using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Services
{
    /// <summary>
    /// Provides facilities to register a service.
    /// <para>
    /// When registering a service, your service must implement an interface in an assembly
    /// outside of the plugin composable. 
    /// Otherwise, your service will never be accessible by any consumer.
    /// </para>
    /// </summary>
    public interface IServiceRegistrationProvider
    {
        /// <summary>
        /// Registers a service with the current service singleton
        /// </summary>
        /// <typeparam name="T">The type of service to register</typeparam>
        /// <param name="serviceInstance">The instance of the service</param>
        void RegisterService<T>(T serviceInstance) where T : class;
    }
}
