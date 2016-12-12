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
        private Dictionary<string, Func<dynamic, object>> Expressions { get; }


        public EndpointCollection()
        {
            this.Expressions = new Dictionary<string, Func<dynamic, object>>();
        }


        public RequestResponse Invoke(string endpointName)
        {
            var param = this.FindParameters(endpointName);
            try
            {
               var response = param.Item2.Invoke(param.Item1);
               if(response is RequestError ) return new RequestResponse(null, response);
                return new RequestResponse(response, null);
            }catch(Exception e){
                return new RequestResponse(null, new RequestError(e));
            }
        }

        public void Add(string endpointPath, Func<dynamic, object> endpoint)
        {
            this.Expressions.Add(endpointPath, endpoint);
        }

        private (dynamic, Func<dynamic, object>) FindParameters(string requestName)
        {
            var expressionEntry = from endpoint in this.Expressions.Where(p => p.Key.Count(k => k == ':') == requestName.Count(k => k == ':'))
                                  let endpointParam = EndpointCollection.MatchNamespace(endpoint.Key, requestName)
                                  where endpointParam != null
                                  select (endpointParam, endpoint.Value);
            if (!expressionEntry.Any()) return (null, (p) => new UnknownEndpointError(requestName));
            var res = expressionEntry.FirstOrDefault();
            dynamic result = new ExpandoObject();
            foreach (var kvp in res.Item1)
            {
                ((IDictionary<String, Object>)result)[kvp.Key] = kvp.Value; //url parameters are all string
            }
            return (result, res.Item2);
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
