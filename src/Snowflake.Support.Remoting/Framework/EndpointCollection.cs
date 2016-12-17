using Newtonsoft.Json;
using Snowflake.Support.Remoting.Framework.Errors;
using Snowflake.Support.Remoting.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.Remoting.Framework
{
    public class EndpointCollection
    {
        private Dictionary<(RequestVerb verb, string path), Func<EndpointParameters, object>> Expressions { get; }


        public EndpointCollection()
        {
            this.Expressions = new Dictionary<(RequestVerb, string), Func<EndpointParameters, object>>();
        }


        public RequestResponse Invoke(RequestVerb endpointVerb, string endpointName, string postdata)
        {
            var param = this.FindParameters(endpointVerb, endpointName);
            try
            {
                var response = param.Item2.Invoke(new EndpointParameters(param.Item1, JsonConvert.DeserializeObject(postdata ?? "")));
                if (response is RequestError error) return new RequestResponse(null, error);
                return new RequestResponse(response, null);
            } catch (RequestException e) {
                return new RequestResponse(null, e.ToError());
            } catch(Exception e) {
                return new RequestResponse(null, new RequestError(e));
            }
        }

        internal void Add(RequestVerb verb, string endpointPath, Func<EndpointParameters, object> endpoint)
        {
            this.Expressions.Add((verb, endpointPath), endpoint);
        }

        private (IDictionary<string, string> getParams, Func<EndpointParameters, object> function) 
                FindParameters(RequestVerb requestVerb, string requestName)
        {
            var expressionEntry = from endpoint in this.Expressions.Where(p => p.Key.path.Count(k => k == ':') == 
                                                    requestName.Count(k => k == ':'))
                                  where endpoint.Key.verb == requestVerb
                                  let endpointParam = EndpointCollection.MatchNamespace(endpoint.Key.path, requestName)
                                  where endpointParam != null
                                  select new ValueTuple<IDictionary<string, string>, Func<EndpointParameters, object>>(endpointParam, endpoint.Value);
            if (!expressionEntry.Any()) return new ValueTuple<IDictionary<string, string>, Func<EndpointParameters, object>>
                    (null, (p) => new UnknownEndpointError(requestVerb, requestName));
            return expressionEntry.FirstOrDefault();
        }

        public static IDictionary<string, string> MatchNamespace(string endpoint, string request)
        {
            string[] endpointNodes = endpoint.Split(':');
            string[] requestNodes = request.Split(':');
            if (endpointNodes.Length != requestNodes.Length) return null; //must have the same amount of nodes.
            var nodeParams = new Dictionary<string, string>();
            for (int i = 0; i < endpointNodes.Length; i++)
            {
                if (endpointNodes[i].StartsWith("{"))
                {
                    string paramName = endpointNodes[i].TrimStart('{').TrimEnd('}');
                    nodeParams[paramName] = requestNodes[i];
                    continue;
                }
                if (endpointNodes[i] != requestNodes[i]) return null;
            }
            return nodeParams;
        }
    }
}
