using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Services
{
    /// <summary>
    /// Enumerates all available services.
    /// </summary>
    public interface IServiceEnumerator
    {
        /// <summary>
        /// Gets the type names of all available services.
        /// </summary>
        IEnumerable<string> Services { get; }
    }
}
