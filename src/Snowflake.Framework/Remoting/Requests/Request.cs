using Snowflake.Remoting.Marshalling;
using Snowflake.Remoting.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.Requests
{
    public class Request : IRequest
    {
        public IRequestPath RequestPath { get; }
        public IEnumerable<IEndpointArgument> EndpointArguments { get; }
        public EndpointVerb Verb { get; }

        public Request(IRequestPath path, EndpointVerb verb,
            IEnumerable<IEndpointArgument> arguments)
        {
            this.Verb = verb;
            this.RequestPath = path;
            this.EndpointArguments = arguments;
        }
    }
}
