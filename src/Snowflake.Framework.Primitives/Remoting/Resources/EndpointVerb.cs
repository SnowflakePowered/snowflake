using System;

namespace Snowflake.Remoting.Resources
{
    public enum EndpointVerb
    {
        /// <summary>
        ///  Equivalent to HTTP POST
        /// </summary>
        Create,
        /// <summary>
        /// Equivalent to HTTP GET
        /// </summary>
        Read,
        /// <summary>
        /// Equivalent to HTTP PUT
        /// </summary>
        Update,
        /// <summary>
        /// Equivalent to HTTP DELETE
        /// </summary>
        Delete
    }
}
