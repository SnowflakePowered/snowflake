using System;
using System.Collections.Generic;
using Snowflake.Loader;

namespace Snowflake.Services
{
    /// <summary>
    /// The core frontend service that handles all the functions of the frontend core.
    /// Acts as a dependency injection container for all <see cref="IComposable"/> modules.
    /// An instance of <see cref="IServiceContainer"/> can not be directly obtained. Instead,
    /// services within it must be requested using <see cref="ImportServiceAttribute"/>, and
    /// new services can be registered using the <see cref="IServiceRegistrationProvider"/> service.
    /// </summary>
    internal interface IServiceContainer : IDisposable
    {
        /// <summary>
        /// Gets the directory to store appdata in this core service
        /// </summary>
        string AppDataDirectory { get; }

        /// <summary>
        /// Register a new singleton service.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="serviceInstance">The instance of the service.</param>
        /// <exception cref="ArgumentException">Thrown when a service of the same type already exists.</exception>
        void RegisterService<T>(T serviceInstance)
            where T : class;

        /// <summary>
        /// Get a list of registered services
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> AvailableServices();

        /// <summary>
        /// Get a service.
        /// The <see cref="IModuleEnumerator"/> service will always be available.
        /// </summary>
        /// <typeparam name="T">The type of service.</typeparam>
        /// <returns>The service instance</returns>
        T Get<T>()
            where T: class;

        /// <summary>
        /// Get a service.
        /// The <see cref="IModuleEnumerator"/> service will always be available.
        /// </summary>
        /// <param name="serviceType">The type of service.</param>
        /// <returns>The service instance</returns>
        object Get(Type serviceType);
    }
}
