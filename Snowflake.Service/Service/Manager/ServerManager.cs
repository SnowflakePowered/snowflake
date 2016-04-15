using System;
using System.Collections.Generic;
using System.Linq;
using Snowflake.Service.HttpServer;

namespace Snowflake.Service.Manager
{
    internal class ServerManager : IServerManager
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

        bool disposed;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
            {
                foreach (string server in this.RegisteredServers)
                {
                    try
                    {
                        this.StopServer(server);
                    }
                    catch (ObjectDisposedException)
                    {
                        continue;
                    }
                }
                this.servers = null;
            }
            this.disposed = true;
        }
    }
}
