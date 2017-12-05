using System;
using System.Collections.Generic;
using Snowflake.Platform;

namespace Snowflake.Services
{
    /// <summary>
    /// The core frontend service that handles all the functions of the frontend core.
    /// </summary>
    public interface IServiceContainer : IDisposable
    {
        /// <summary>
        /// Gets the directory to store appdata in this core service
        /// </summary>
        string AppDataDirectory { get; }

        /// <summary>
        /// Register a service with this coreservice
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceInstance"></param>
        void RegisterService<T>(T serviceInstance);

        /// <summary>
        /// Get a list of registered services
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> AvailableServices();

        /// <summary>
        /// Get a service.
        /// The <see cref="IStoneProvider"/> service will always be available.
        /// </summary>
        /// <typeparam name="T">The type of service.</typeparam>
        /// <returns>The service instance</returns>
        T Get<T>();
    }
}
