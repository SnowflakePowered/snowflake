using System.Collections.Generic;
using Snowflake.Input.Controller.Mapped;

namespace Snowflake.Input
{
    /// <summary>
    /// Represents a store of controller mapping profiles <see cref="IMappedControllerElementCollection"/>
    ///
    /// <para>
    /// Mapping profiles are stored in standard Stone controller format in a database, keyed on
    /// the Stone layout name of the emulated controller,
    /// the device name of the enumerated real controller <see cref="Input.Device.IInputDevice"/>,
    /// and a profile name, user specified and 'default'.
    /// </para>
    ///
    /// <para>
    /// The IMappedControllerElementCollectionStore does not consider which player and only stores
    /// a list of profiles for each combination of real to emulated controller.
    /// </para>
    /// </summary>
    public interface IMappedControllerElementCollectionStore
    {
        /// <summary>
        /// Gets a corresponding mapping profile for the provided layout, device, and profile.
        /// </summary>
        /// <param name="controllerId">The layout or controller ID of the emulated controller</param>
        /// <param name="deviceId">The enumerated ID of the input device</param>
        /// <param name="profileName">The name of the profile. Should none be specified, the default profile</param>
        /// <returns>The mapping profile that maps emulated controller button elements to the device layout</returns>
        IMappedControllerElementCollection GetMappingProfile(string controllerId, string deviceId, string profileName = "default");

        /// <summary>
        /// Gets saved profile names for a certain combination of an emulated controller and an input device
        /// </summary>
        /// <param name="controllerId">The layout or controller ID of the emulated controller</param>
        /// <param name="deviceId">The enumerated ID of the input device</param>
        /// <returns>Saved profile names for this combination</returns>
        IEnumerable<string> GetProfileNames(string controllerId, string deviceId);

        /// <summary>
        /// Sets the corresponding mapping profile for the provided profile name.
        /// Replaces the entire mapping profile, without checking if it is complete.
        /// </summary>
        /// <param name="mappedCollection">The mapping profile to set, including the layout and controller ID</param>
        /// <param name="profileName">The name of the profile. Should none be specified, the default profile</param>
        void SetMappingProfile(IMappedControllerElementCollection mappedCollection, string profileName = "default");
    }
}