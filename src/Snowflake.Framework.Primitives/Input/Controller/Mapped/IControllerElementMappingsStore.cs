using System.Collections.Generic;
using Snowflake.Input.Controller.Mapped;

namespace Snowflake.Input.Controller.Mapped
{
    /// <summary>
    /// Provides a store for <see cref="IControllerElementMappings"/>.
    /// </summary>
    public interface IControllerElementMappingsStore
    {
        /// <summary>
        /// Add the given mappings under the given profile name. Profile names do not have to be unique
        /// across mappings between different spec controllers and real devices, but must be unique
        /// for the same controller ID and device ID.
        /// </summary>
        /// <param name="mappings">The <see cref="IControllerElementMappings"/> to store.</param>
        /// <param name="profileName">The profile name to store the mappings under.</param>
        void AddMappings(IControllerElementMappings mappings, string profileName);
        
        /// <summary>
        /// Deletes all mappings from the provided controller ID to device ID.
        /// </summary>
        /// <param name="controllerId">The Stone controller ID that maps to the real device.</param>
        /// <param name="deviceId">The device ID that maps from the spec controller.</param>
        void DeleteMappings(ControllerId controllerId, string deviceId);

        /// <summary>
        /// Deletes the mapping profile from the provided controller ID to device ID.
        /// </summary>
        /// <param name="controllerId">The Stone controller ID that maps to the real device.</param>
        /// <param name="deviceId">The device ID that maps from the spec controller.</param>
        /// <param name="profileName">The name of the mapping profile.</param>
        void DeleteMappings(ControllerId controllerId, string deviceId, string profileName);
        
        /// <summary>
        /// Gets all saved mappings from the provided controller ID to device ID.
        /// </summary>
        /// <param name="controllerId">The Stone controller ID that maps to the real device.</param>
        /// <param name="deviceId">The device ID that maps from the spec controller.</param>
        /// <returns>All saved mappings from the provided controller ID to device ID.</returns>
        IEnumerable<IControllerElementMappings> GetMappings(ControllerId controllerId, string deviceId);

        /// <summary>
        /// Gets the saved mapping profile from the provided controller ID to device ID.
        /// </summary>
        /// <param name="controllerId">The Stone controller ID that maps to the real device.</param>
        /// <param name="deviceId">The device ID that maps from the spec controller.</param>
        /// <param name="profileName">The name of the mapping profile.</param>
        /// <returns>The saved mapping profile from the provided controller ID to device ID.</returns>
        IControllerElementMappings? GetMappings(ControllerId controllerId, string deviceId, string profileName);
        
        /// <summary>
        /// Updates the specific mapping profile with the given profile name.
        /// </summary>
        /// <param name="mappings">The <see cref="IControllerElementMappings"/> to store.</param>
        /// <param name="profileName">The profile name to store the mappings under.</param>
        void UpdateMappings(IControllerElementMappings mappings, string profileName);
    }
}
