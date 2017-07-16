using System.Collections.Generic;
using Snowflake.Remoting.Marshalling;
using Snowflake.Remoting.Resources;

namespace Snowflake.Remoting.Requests
{
    public interface IRequest
    {
        IEnumerable<IEndpointArgument> EndpointArguments { get; }
        IRequestPath RequestPath { get; }
        EndpointVerb Verb { get; }
    }
}