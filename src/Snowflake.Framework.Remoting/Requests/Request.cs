using Snowflake.Framework.Remoting.Marshalling;
using Snowflake.Framework.Remoting.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.Requests
{
    public class Request
    {
        public RequestPath RequestPath { get; }
        public IEnumerable<EndpointArgument> EndpointArguments { get; }
        public EndpointVerb Verb { get; }
        public Request(RequestPath path, EndpointVerb verb,
            IEnumerable<EndpointArgument> arguments)
        {
            this.Verb = verb;
            this.RequestPath = path;
            this.EndpointArguments = arguments;
        }
    }
}
