using System.Collections.Generic;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;

namespace Snowflake.Configuration.Input
{
    /// <summary>
    /// Represents an input configuration template from which valid input configuration
    /// can be serialized. The implementation of which is a wrapper around an interface that implements this interface.
    /// </summary>
    /// <typeparam name="T">The type of input configuration</typeparam>
    public interface IInputTemplate<out T> : IInputTemplate, IConfigurationSection<T>
        where T : class
    {
        /// <summary>
        /// Gets the template in which the values are stored for this input configuration
        /// </summary>
        T Template { get; }
    }

    /// <summary>
    /// Represents an input configuration template from which valid input configuration
    /// can be serialized.
    /// </summary>
    public interface IInputTemplate : IConfigurationSection
    {
        /// <summary>
        /// Gets the controller index of this template instance.
        /// This is zero indexed, Player 1 for example is index 0.
        /// </summary>
        int PlayerIndex { get; }

        /// <summary>
        /// Gets the mapped controller elements of the input configuration.
        /// A proper implementation should require this be immutable.
        /// This dictionary is keyed on the property names of the input template interface.
        /// </summary>
        new IReadOnlyDictionary<string, DeviceCapability> Values { get; }

        /// <summary>
        /// Gets the options representing the fields in which the options are serialized in configuration,
        /// the implementation of this should ensure that this is enumerated in the same order
        /// with which the properties are described.
        /// </summary>
        IEnumerable<IInputOption> Options { get; }

        /// <summary>
        /// Gets or sets the real device element that has been mapped to a virtual element.
        /// 
        /// This will set all matching options for every <see cref="IInputOption"/> that matches the provided
        /// <see cref="IInputOption.TargetElement"/> and the provided <see cref="DeviceCapability"/>,
        /// depending on the <see cref="DeviceCapabilityClass"/> of the option.
        /// </summary>
        /// <param name="targetElement">The virtual element on the controller definition</param>
        DeviceCapability this[ControllerElement targetElement] { set; }
    }
}
