using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting
{
    public enum RemotingVerbs
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
