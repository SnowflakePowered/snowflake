using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service.HttpServer;
namespace Snowflake.Service.Manager
{
    public class ServerManager : IServerManager
    {
        private IDictionary<string, IBaseHttpServer> servers;
        public ServerManager()
        {
            this.servers = new Dictionary<string, IBaseHttpServer>();
        }
        public void RegisterServer(string serverName, IBaseHttpServer httpServer)
        {
            this.servers.Add(serverName, httpServer);
        }

        public void StartServer(string serverName)
        {
            this.servers[serverName].StartServer();
        }

        public void StopServer(string serverName)
        {
            this.servers[serverName].StopServer();
        }

        public IList<string> RegisteredServers
        {
            get 
            {
                return this.servers.Select(server => server.Key).ToList();
            }
        }
        public IBaseHttpServer GetServer(string serverName)
        {
            return this.servers[serverName];
        }
        public IBaseHttpServer this[string serverName] => this.GetServer(serverName);

        public void Dispose()
        {
            foreach (var server in this.RegisteredServers)
            {
                this.StopServer(server);
                this.servers[server] = null;
            }
            this.servers = null;
        }
    }
}
