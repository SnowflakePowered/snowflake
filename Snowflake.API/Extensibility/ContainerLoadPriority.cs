using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Extensibility
{
    /// <summary>
    /// Represents the priority with which the plugin manager will call compose on a container.
    /// Containers with the same priority are not guaranteed to compose at the same priority every time. 
    /// If you need more fine control, use the PluginLoadEvent.
    /// </summary>
    public enum ContainerLoadPriority
    {

        /// <summary>
        /// A service container only initializes services that are crucial for other plugins.
        /// It has no dependencies of any kind, no configuration, and registers to the core instance without
        /// having a dependency on any other core instance service.
        /// </summary>
        Service,

        /// <summary>
        /// A high priority container initializes after all services have initialized, but other plugins have not.
        /// For example, a service that depends on another service here would use the high level.
        /// </summary>
        High,

        /// <summary>
        /// A medium priority container initializes after all all service level and high level containers intialize.
        /// A plugin that expects services would use this.
        /// </summary>
        Medium,

        /// <summary>
        /// A low priority container initializes after all service, high, and medium level plugins would intialize.
        /// For example, plugins that tightly depends on other plugins would use this.
        /// </summary>
        Low,

        /// <summary>
        /// Containers without a PluginContainerAttribute or those marked with the last priority will be initialized last.
        /// </summary>
        Default
    }
}
