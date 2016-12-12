using Newtonsoft.Json;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Unosquare.Labs.EmbedIO;
using System.Linq;
using System.Linq.Expressions;
using Snowflake.Support.Remoting.Resources;
using System.Dynamic;

namespace Snowflake.Support.Remoting.Servers
{
    public class RestRemotingServer : WebModuleBase
    {
        public override string Name => "Snowflake REST Remoting";
        public Dictionary<string, Func<dynamic, object>> Expressions { get; }

        public RestRemotingServer(ICoreService coreService)
        {
            var pluginEndpoint = new Plugins(coreService);
            this.Expressions = new Dictionary<string, Func<dynamic, object>>();
            this.Expressions.Add("~:plugins", (p) => pluginEndpoint.ListPlugins());
            this.Expressions.Add("~:plugins:{echo}", (p) => pluginEndpoint.Echo(p.echo));
            this.AddHandler(ModuleMap.AnyPath, HttpVerbs.Any, (server, context) =>
            {
                var verb = context.RequestVerb();
                context.NoCache();
                context.Response.ContentType = "application/json";
                var split = context.RequestPath().Split('/');
                var str = new StringBuilder();
                str.Append($"Verb: {verb}, Path: {ToEndpointName(context.RequestPath())} \n");
                var param = FindParameters(ToEndpointName(context.RequestPath()));
                foreach(var p in param.Item1)
                {
                    str.AppendLine("Param: " + p.Key + ": " + p.Value);
                }
                str.Append(param.Item2.Invoke(param.Item1));
                var buffer = Encoding.UTF8.GetBytes(str.ToString());
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                return true;
            });
        }
        
        private (dynamic, Func<dynamic,object>) FindParameters(string requestName)
        {
            var expressionEntry = from endpoint in this.Expressions.Where(p => p.Key.Count(k => k == ':') == requestName.Count(k => k == ':'))
                                  let endpointParam = MatchNamespace(endpoint.Key, requestName)
                                  where endpointParam != null
                                  select (endpointParam, endpoint.Value);
            var res = expressionEntry.FirstOrDefault();
            dynamic result = new ExpandoObject();
            
            foreach(var kvp in res.Item1)
            {
                ((IDictionary<String, Object>)result)[kvp.Key] = kvp.Value;
            }
            return (result, res.Item2);
        }

        private static string ToEndpointName(string path)
        {
            return $"~:{string.Join(":", path.Split('/').Where(s => s.Length > 0))}";
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
