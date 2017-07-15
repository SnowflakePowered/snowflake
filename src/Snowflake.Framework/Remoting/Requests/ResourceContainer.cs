using Snowflake.Remoting.Marshalling;
using Snowflake.Remoting.Resources;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Collections;

namespace Snowflake.Remoting.Requests
{
    public class ResourceContainer : IResourceContainer, IEnumerable<IResource>
    {
        public IEnumerable<IResource> Resources => this.resources;
        private ImmutableList<IResource> resources;
        private readonly IArgumentTypeMapper typeMappper;
        //todo pluggable mappers!!!
        public ResourceContainer(IArgumentTypeMapper typeMapper)
        {
            this.resources = ImmutableList.Create<IResource>();
            this.typeMappper = typeMapper;

        }
        public ResourceContainer() : this(ArgumentTypeMapper.Default)
        {
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
                return new RequestResponse(null, ResponseStatus.NotFoundStatus(request.Verb, request.RequestPath));
            }
            try
            {
                var result = resource.Execute(matched, this.typeMappper
                    .CastArguments(pathArgs, endArgs));
                return new RequestResponse(result, ResponseStatus.OkStatus(request.Verb, request.RequestPath));
            }
            catch (RequestException e)
            {
                return new RequestResponse(null, ResponseStatus.UnhandledErrorStatus(request.Verb, request.RequestPath, e, e.ErrorCode));
            }
            catch (Exception e)
            {
                //todo: properly report this.
                return new RequestResponse(null, ResponseStatus.UnhandledErrorStatus(request.Verb, request.RequestPath, e));
            }
        }

        public IEnumerator<IResource> GetEnumerator()
        {
            return this.Resources.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
