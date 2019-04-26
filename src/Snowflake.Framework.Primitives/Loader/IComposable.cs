using System.Reflection;
using Snowflake.Services;

namespace Snowflake.Loader
{
    /// <summary>
    /// A container for plugins to initialize in.
    /// All composable objects must implement this interface, and register their plugins inside the
    /// Compose method.
    /// </summary>
    public interface IComposable
    {
        /// <summary>
        /// This method is called upon initialization of your plugin assembly.
        /// In this method, initialize your plugin objects and register them to the plugin manager to expose access to Snowflake.
        /// </summary>
        /// <param name="composableModule">The module metadata of the loading module.</param>
        /// <param name="serviceContainer">The service container that is injected by the plugin manager.</param>
        /// <see cref="IPluginManager.Register{T}(T)"/>
        void Compose(IModule composableModule, IServiceRepository serviceContainer);
    }
}
