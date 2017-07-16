using System.Collections.Generic;
using Snowflake.Remoting.Resources;

namespace Snowflake.Remoting.Requests
{
    public interface IResourceContainer
    {
        IEnumerable<IResource> Resources { get; }
        void Add(IResource resource);
        IRequestResponse ExecuteRequest(IRequest request);
        IResource MatchResource(IRequestPath requestPath);
    }
}