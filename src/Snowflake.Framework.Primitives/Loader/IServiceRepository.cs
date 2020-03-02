using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Loader
{
    /// <summary>
    /// Provides access to imported services.
    /// </summary>
    public interface IServiceRepository
    {
        /// <summary>
        /// Gets the given service
        /// </summary>
        /// <typeparam name="T">The requested service</typeparam>
        /// <returns>The available services</returns>
        /// <exception cref="InvalidOperationException">
        /// The service has not been imported or you do not have permissions to access the service.
        /// </exception>
        T Get<T>()
            where T : class;

        /// <summary>
        /// Gets a list of available service types.
        /// </summary>
        IEnumerable<string> Services { get; }
        
        /// <summary>
        /// Returns a <see cref="IServiceProvider"/> that includes the services available in this instance of <see cref="IServiceRepository"/>.
        /// The returned <see cref="IServiceProvider"/> has different usage semantics for <see cref="IServiceProvider.GetService(Type)"/> than
        /// <see cref="IServiceRepository.Get{T}"/>, namely, getting a service that has not been authorized for this <see cref="IServiceRepository"/>,
        /// or one that does not exist, will return null instead of throwing <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <returns>A <see cref="IServiceProvider"/> that contain all available services in this instance of <see cref="IServiceRepository"/>.</returns>
        IServiceProvider ToServiceProvider();
    }
}
