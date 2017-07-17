using System.Collections.Generic;
using Snowflake.Remoting.Marshalling;

namespace Snowflake.Remoting.Resources
{
    public interface IResource
    {
        IEnumerable<IMethodEndpoint> Endpoints { get; }
        IResourcePath Path { get; }

        object Execute(IMethodEndpoint endpoint, IEnumerable<ITypedArgument> typedArgs);
        IMethodEndpoint MatchEndpointWithParams(EndpointVerb verb, IEnumerable<IEndpointArgument> requestArguments);
    }
}