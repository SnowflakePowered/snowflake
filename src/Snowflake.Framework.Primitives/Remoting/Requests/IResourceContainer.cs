using System.Collections.Generic;
using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Marshalling;

namespace Snowflake.Remoting.Requests
{
    public interface IResourceContainer
    {
        IEnumerable<IResource> Resources { get; }
        void Add(IResource resource);
        void AddTypeMapping<T>(ITypeMapping<T> typeMapping);
        IRequestResponse ExecuteRequest(IRequest request);
        IResource MatchResource(IRequestPath requestPath);
    }
}