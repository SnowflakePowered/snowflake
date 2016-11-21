namespace Snowflake.Service.HttpServer
{
    /// <summary>
    /// Represents an HttpServer that can be used to serve content.
    /// <see cref="Snowflake.Server.HttpServer.Process(Mono.Net.HttpListenerContext)"/> on how to implement the server loop
    /// </summary>
    public interface IBaseHttpServer 
    {
        void StartServer();
        void StopServer();
    }
}
