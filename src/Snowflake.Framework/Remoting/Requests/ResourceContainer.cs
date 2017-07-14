using Snowflake.Remoting.Marshalling;
using Snowflake.Remoting.Resources;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Snowflake.Remoting.Requests
{
    public class ResourceContainer : IResourceContainer
    {
        public IEnumerable<IResource> Resources => this.resources;
        private ImmutableList<IResource> resources;

        public ResourceContainer()
        {
            this.resources = ImmutableList.Create<IResource>();
        }

        public void Add(IResource resource)
        {
            this.resources = this.resources.Add(resource);
        }
        
        public IResource MatchResource(IRequestPath requestPath)
        {
            return this.resources.Where(r => r.Path.Match(requestPath)).FirstOrDefault();
        }

        public IRequestResponse ExecuteRequest(IRequest request)
        {
            var resource = this.MatchResource(request.RequestPath);
            var pathArgs = resource?.Path.MatchArguments(request.RequestPath);
            var matched = resource?.MatchEndpoint(request.Verb, request.EndpointArguments);
            var endArgs = matched.MatchArguments(request.EndpointArguments);
            if (matched == null)
            {
                return RequestResponse.NoEndpointFoundResponse;
            }
            try
            {
                var result = resource.Execute(matched, ArgumentTypeMapper.Default
                    .CastArguments(pathArgs, endArgs));
                return new RequestResponse(result);
            }
            catch (Exception e)
            {
                //todo: properly report this.
                return RequestResponse.NoEndpointFoundResponse;
            }
        }
    }
}
