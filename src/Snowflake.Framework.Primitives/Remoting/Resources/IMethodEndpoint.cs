using System.Collections.Generic;
using System.Reflection;
using Snowflake.Remoting.Marshalling;

namespace Snowflake.Remoting.Resources
{
    public interface IMethodEndpoint
    {
        MethodInfo EndpointMethodInfo { get; }
        IEnumerable<IParameter> EndpointParameters { get; }
        EndpointVerb Verb { get; }

        IEnumerable<ISerializedArgument> SerializeEndpointArguments(IEnumerable<IEndpointArgument> args);
    }
}