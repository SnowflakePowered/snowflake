using Newtonsoft.Json;
using Snowflake.Support.Remoting.Framework.Errors;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.Remoting.Framework
{
    public class EndpointCollection
    {
        private Dictionary<string, Func<EndpointParameters, object>> Expressions { get; }


        public EndpointCollection()
        {
            this.Expressions = new Dictionary<string, Func<EndpointParameters, object>>();
        }


        public RequestResponse Invoke(string endpointName, string postdata)
        {
            var param = this.FindParameters(endpointName);
            try
            {
               var response = param.Item2.Invoke(new EndpointParameters(param.Item1, JsonConvert.DeserializeObject(postdata)));
               if(response is RequestError error) return new RequestResponse(null, error);
               return new RequestResponse(response, null);
            }catch(Exception e){
                return new RequestResponse(null, new RequestError(e));
            }
        }

        public void Add(string endpointPath, Func<EndpointParameters, object> endpoint)
        {
            this.Expressions.Add(endpointPath, endpoint);
        }

        private ValueTuple<IDictionary<string, string>, Func<EndpointParameters, object>> FindParameters(string requestName)
        {
            var expressionEntry = from endpoint in this.Expressions.Where(p => p.Key.Count(k => k == ':') == requestName.Count(k => k == ':'))
                                  let endpointParam = EndpointCollection.MatchNamespace(endpoint.Key, requestName)
                                  where endpointParam != null
                                  select new ValueTuple<IDictionary<string, string>, Func<EndpointParameters, object>>(endpointParam, endpoint.Value);
            if (!expressionEntry.Any()) return new ValueTuple<IDictionary<string, string>, Func<EndpointParameters, object>>(null, (p) => new UnknownEndpointError(requestName));
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
