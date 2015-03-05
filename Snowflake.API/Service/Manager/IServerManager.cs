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
        public void RegisterServer(string serverName, IBaseHttpServer httpServer);
        public void StartServer(string serverName);
        public void StopServer(string serverName);
        public IList<string> RegisteredServers { get; }
    }
}
