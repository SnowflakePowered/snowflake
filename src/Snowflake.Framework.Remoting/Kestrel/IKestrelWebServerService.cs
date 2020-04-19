using Snowflake.Loader;
using System.Threading.Tasks;

namespace Snowflake.Remoting.Kestrel
{
    /// <summary>
    /// Provides middleware access to the Kestrel remoting server.
    /// </summary>
    public interface IKestrelWebServerService
    {
        /// <summary>
        /// Registers a Kestrel middleware.
        /// Kestrel middleware can only be registered within <see cref="IComposable.Compose(IModule, IServiceRepository)"/>.
        /// Once assembly composition finishes, middleware can no longer be added.
        /// </summary>
        /// <typeparam name="T">The type of the class that implements the middleware.</typeparam>
        /// <param name="kestrelServerMiddleware">The middleware to register to the Kestrel server.</param>
        void AddService<T>(T kestrelServerMiddleware) where T : IKestrelServerMiddlewareProvider;
    }
}