using System.Threading.Tasks;

namespace Snowflake.Framework.Remoting.Kestrel
{
    public interface IKestrelWebServerService
    {
        void AddService<T>(T kestrelServerMiddleware) where T : IKestrelServerMiddlewareProvider;
    }
}