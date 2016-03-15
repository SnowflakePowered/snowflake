using System.ComponentModel.Composition;
using System.Reflection;
using Snowflake.Service;
using Snowflake.Service.Manager;

namespace Snowflake.Extensibility
{

    /// <summary>
    /// A container for plugins to initialize in. 
    /// All composable objects must implement this interface, and register their plugins inside the
    /// Compose method.
    /// </summary>
    [InheritedExport(typeof(IPluginContainer))]
    public interface IPluginContainer
    {
        /// <summary>
        /// This method is called upon initialization of your plugin assembly. 
        /// In this method, initialize your plugin objects and register them to the plugin manager to expose access to Snowflake.
        /// </summary>
        /// <param name="coreInstance">The core instance that is injected by the plugin manager</param>
        /// <see cref="IPluginManager.Register{T}(T)"/>
        void Compose(ICoreService coreInstance);
    }
}
