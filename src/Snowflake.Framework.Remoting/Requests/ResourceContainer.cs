using Snowflake.Framework.Remoting.Marshalling;
using Snowflake.Framework.Remoting.Resources;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Snowflake.Framework.Remoting.Requests
{
    public class ResourceContainer
    {
        public IEnumerable<Resource> Resources => this.resources;
        private ImmutableList<Resource> resources;

        public ResourceContainer()
        {
            this.resources = ImmutableList.Create<Resource>();
        }

        public void Add(Resource resource)
        {
            this.resources = this.resources.Add(resource);
        }
        
        public Resource MatchResource(RequestPath requestPath)
        {
            return this.resources.Where(r => r.Path.Match(requestPath)).FirstOrDefault();
        }

        public Response ExecuteRequest(Request request)
        {
            var resource = this.MatchResource(request.RequestPath);
            var pathArgs = resource?.Path.MatchArguments(request.RequestPath);
            var matched = resource?.MatchEndpoint(request.Verb, request.EndpointArguments);
            var endArgs = matched.MatchArguments(request.EndpointArguments);
            if (matched == null)
            {
                return Response.NoEndpointFoundResponse;
            }
            try
            {
                var result = resource.Execute(matched, ArgumentTypeMapper.Default
                    .CastArguments(pathArgs, endArgs));
                return new Response(result);
            }
            catch (Exception e)
            {
                //todo: properly report this.
                return Response.NoEndpointFoundResponse;
            }
        }
    }
}
