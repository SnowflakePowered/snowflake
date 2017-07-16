using Snowflake.Services;
using Snowflake.Support.Remoting.Servers;
using System;
using System.Collections.Generic;
using System.Text;
using Unosquare.Labs.EmbedIO;

namespace Snowflake.Servers
{
    public class WebServerWrapper : ILocalWebService
    {
        private RestRemotingServer remote;
        public WebServerWrapper(RestRemotingServer remote)
        {
            this.remote = remote;

        }

        public int Port => 9696;

        public string Protocol => "http";

        public string Name => "Remoting";

        public void Start()
        {
            var webServer = new WebServer("http://localhost:9696/");
            webServer.RegisterModule(remote);

            webServer.RunAsync();
        }
    }
}
