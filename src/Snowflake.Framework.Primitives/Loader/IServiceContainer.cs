using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Loader
{
    /// <summary>
    /// Provides access to imported services.
    /// </summary>
    public interface IServiceProvider
    {
        /// <summary>
        /// Gets the given service
        /// </summary>
        /// <typeparam name="T">The requested service</typeparam>
        /// <returns>The available services</returns>
        T Get<T>() where T : class;
        /// <summary>
        /// Gets a list of available service types.
        /// </summary>
        IEnumerable<string> Services { get; }
    }
}
