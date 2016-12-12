using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework.Errors
{
    internal class UnknownEndpointError : RequestError
    {
        public UnknownEndpointError(string unknownEndpoint) : base($"Could not find endoint {unknownEndpoint}", 404)
        {

        }
    }
}
