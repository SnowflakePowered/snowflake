using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Input.Controller;
namespace Snowflake.Configuration
{
    /// <summary>
    /// Represents an input configuration.
    /// Inherit from this class and add your own properties, which will be serialized to a configuration file.
    /// </summary>
    //todo extend this doc
    public interface IInputConfigurationSection : IIterableConfigurationSection
    {
        /// <summary>
        /// The input device associated with this input configuration
        /// </summary>
        IInputDevice InputDevice { get; }

        /// <summary>
        /// The virtual device associated with this device
        /// </summary>
        IControllerLayout VirtualDevice { get; }

        /// <summary>
        /// The controller mapping associated with this device  
        /// </summary>
        IMappedControllerElementCollection ControllerMapping { get; }
    }
}
