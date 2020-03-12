using Snowflake.Loader;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Snowflake.Services
{
    /// <summary>
    /// Provides facilities to register a service.
    /// <para>
    /// A service must implement an interface that resides outside of its implementing assembly. A service that implements
    /// an interface in its own assembly will not be accessible by any consumer due to resolution rules. This is true even
    /// for a service that only exposes a private API. In such case, it is advisable to expose an internal API, and use
    /// <see cref="InternalsVisibleToAttribute"/> to expose the interface.
    /// </para>
    /// </summary>
    public interface IServiceRegistrationProvider
    {
        /// <summary>
        /// Registers a singleton service with the service container, in order to be accessible to other incoming
        /// <see cref="IComposable"/>s. <see cref="IComposable"/> modules are loaded in dependency order; until
        /// all dependencies as specified using <see cref="ImportServiceAttribute"/> attributes are satisfiable
        /// by the loader, the composable will not be loaded. Hence it is important that services be registered 
        /// this way.
        /// <para>
        /// A service must implement an interface that resides outside of its implementing assembly. A service that implements
        /// an interface in its own assembly will not be accessible by any consumer due to resolution rules; this is true even
        /// for a service that only exposes a private API. In such case, it is advisable to manually manage the lifetime of
        /// your service, or expose an internal API, and use <see cref="InternalsVisibleToAttribute"/> to expose the interface
        /// to another assembly. There is no restriction on <em>module</em> however; registering an instance of an interface that
        /// resides in a different assembly, but in the same module as the registering asembly, is perfectly fine.
        /// </para>
        /// </summary>
        /// <typeparam name="T">The type of the service to register.</typeparam>
        /// <param name="serviceInstance">The instance of the service to register.</param>
        /// <exception cref="ArgumentException">Thrown when a service of the same type was already registered.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a service and its implementing type reside in the same assembly.
        /// </exception>
        void RegisterService<T>(T serviceInstance)
            where T : class;
    }
}
