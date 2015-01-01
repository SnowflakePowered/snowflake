using System;
namespace Snowflake.Service.HttpServer
{
    public interface IBaseHttpServer
    {
        void StartServer();
        void StopServer();
    }
}
