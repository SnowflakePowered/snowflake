using Newtonsoft.Json;
using Snowflake.Services;
using Snowflake.Support.Remoting.Endpoints;
using System;
using System.Collections.Generic;
using System.Text;
using Unosquare.Labs.EmbedIO;

namespace Snowflake.Support.Remoting.Servers
{
    public class RestRemotingServer : WebModuleBase
    {
        public override string Name => "Snowflake REST Remoting";

        private Dictionary<string, Func<>>
        public RestRemotingServer(ICoreService coreService)
        {
            this.AddHandler(ModuleMap.AnyPath, HttpVerbs.Any, (server, context) =>
            {
                var verb = context.RequestVerb();
                context.NoCache();
                context.Response.ContentType = "application/json";
                var split = context.RequestPath().Split('/');
                var buffer = Encoding.UTF8.GetBytes($"Verb: {verb}, Path: {context.RequestPath()} \n {JsonConvert.SerializeObject(new Games(coreService))}");
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                return true;
            });
        }
    }
}
