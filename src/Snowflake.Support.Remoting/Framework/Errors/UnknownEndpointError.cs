using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework.Errors
{
    internal class UnknownEndpointError : RequestError
    {
        public UnknownEndpointError(RequestVerb verb, string unknownEndpoint) 
            : base($"Could not find endpoint {verb}::{unknownEndpoint}", 404)
        {

        }
    }
}
