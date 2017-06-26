using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Services
{
    /// <summary>
    /// Provides facilities to register a service.
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
