using System;
using System.Collections.Generic;
using Snowflake.Services.HttpServer;

namespace Snowflake.Services.Manager
{
    public interface IServerManager: IDisposable
    {
        void RegisterServer(string serverName, IBaseHttpServer httpServer);
        void StartServer(string serverName);
        void StopServer(string serverName);
        IList<string> RegisteredServers { get; }
        IBaseHttpServer GetServer(string serverName);
        IBaseHttpServer this[string serverName] { get; }
    }
}
