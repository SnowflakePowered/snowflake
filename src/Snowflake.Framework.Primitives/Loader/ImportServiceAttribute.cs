using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Loader
{
    /// <summary>
    /// Requests for a service to be available during composition in an <see cref="IComposable"/>.
    /// Once all services that are requested are available to be fulfilled,
    /// <see cref="IComposable.Compose(IModule, IServiceRepository)"/> will be called.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class ImportServiceAttribute : Attribute
    {
        /// <summary>
        /// Requests for a service to be available during composition in an <see cref="IComposable"/>.
        /// </summary>
        /// <param name="service">The type of the service to request.</param>
        public ImportServiceAttribute(Type service)
        {
            this.Service = service;
        }

        /// <summary>
        /// The type of the service to request.
        /// </summary>
        public Type Service { get; }
    }
}
