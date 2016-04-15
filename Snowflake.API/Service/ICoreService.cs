using System;
using System.Collections.Generic;
using Snowflake.Platform;

namespace Snowflake.Service
{
    /// <summary>
    /// The core frontend service that handles all the functions of the frontend core.
    /// </summary>
    public interface ICoreService : IDisposable
    {
        /// <summary>
        /// The directory to store appdata in this core service
        /// </summary>
        string AppDataDirectory { get; }
        /// <summary>
        /// Register a service with this coreservice
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceObject"></param>
        void RegisterService<T>(T serviceObject);
        /// <summary>
        /// Get a list of registered services
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> AvailableServices();
        /// <summary>
        /// Get a service.
        /// The StoneProvider service is guaranteed to be registered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>();
    }
}
