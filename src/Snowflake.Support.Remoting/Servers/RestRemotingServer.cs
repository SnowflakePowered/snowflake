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
using Snowflake.Support.Remoting.Framework;
using Newtonsoft.Json.Converters;

namespace Snowflake.Support.Remoting.Servers
{
    public class RestRemotingServer : WebModuleBase
    {
        public override string Name => "Snowflake REST Remoting";
        public Dictionary<string, Func<EndpointParameters, object>> Expressions { get; }

        public RestRemotingServer(EndpointCollection endpoints)
        {
            JsonConvert.DefaultSettings = (() =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter());
                return settings; //stopgap solution
            });

            this.AddHandler(ModuleMap.AnyPath, HttpVerbs.Any, (server, context) =>
            {
                var verb = context.RequestVerb();
                context.NoCache();
                context.Response.ContentType = "application/json";
                var split = context.RequestPath().Split('/');
                var str = new StringBuilder();
                str.Append($"Verb: {verb}, Path: {ToEndpointName(context.Request.RawUrl)} \n");
                str.Append(JsonConvert.SerializeObject(endpoints.Invoke(ToEndpointName(context.Request.RawUrl), context.RequestBody())));
                var buffer = Encoding.UTF8.GetBytes(str.ToString());
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                return true;
            });
        }

        private static string ToEndpointName(string path)
        {
            return $"~:{string.Join(":", path.Split('/').Where(s => s.Length > 0))}";
        }


    }
}
