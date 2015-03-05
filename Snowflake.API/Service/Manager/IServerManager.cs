using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service.HttpServer;
namespace Snowflake.Service.Manager
{
    public interface IServerManager
    {
        void RegisterServer(string serverName, IBaseHttpServer httpServer);
        void StartServer(string serverName);
        void StopServer(string serverName);
        IList<string> RegisteredServers { get; }
    }
}
