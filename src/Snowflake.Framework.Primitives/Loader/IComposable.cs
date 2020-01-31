using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Snowflake.Services;

namespace Snowflake.Loader
{
    /// <summary>
    /// A container for modules and plugins to initialize in.
    /// All composable objects must implement this interface, and register their plugins inside the
    /// Compose method.
    /// </summary>
    public interface IComposable
    {
        /// <summary>
        /// This method is called upon initialization of your plugin assembly.
        /// In this method, initialize your plugin objects and register them to the plugin manager to expose access to Snowflake.
        /// <para>
        /// Access to various services in the <see cref="IServiceRepository"/> must be negotiated for using
        /// <see cref="ImportServiceAttribute"/>. Some important services are <see cref="IPluginManager"/>,
        /// as well as <see cref="IServiceRegistrationProvider"/> to register your own services.
        /// </para>
        /// </summary>
        /// <param name="composableModule">The module metadata of the loading module.</param>
        /// <param name="serviceRepository">The services that are injected by the module compositor.</param>
        /// <see cref="IPluginManager.Register{T}(T)"/>
        void Compose(IModule composableModule, IServiceRepository serviceRepository);
    }
}
