using System;
using System.Collections.Generic;
using Snowflake.Controller;
using Snowflake.Platform;

namespace Snowflake.Service
{
    /// <summary>
    /// The core frontend service that handles all the functions of the frontend core.
    /// </summary>
    public interface ICoreService : IDisposable
    {
        /// <summary>
        /// The list of platforms loaded for this core service
        /// </summary>
        [Obsolete("Use StoneProvider instead. Will be removed in upcoming PR.")]
        IDictionary<string, IPlatformInfo> Platforms { get; }
        /// <summary>
        /// THe list of controllers loaded for this core service
        /// </summary>
        [Obsolete("Use StoneProvider instead. Will be removed in upcoming PR.")]
        IDictionary<string, IControllerDefinition> Controllers { get; }
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
        /// Provides stone platform and controller data
        /// </summary>
        IStoneProvider StoneProvider { get; }
        /// <summary>
        /// Get a service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>();
        /// <summary>
        /// Dispose the core
        /// </summary>
        void Dispose();

    }
}
