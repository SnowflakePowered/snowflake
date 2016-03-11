using System.Collections.Generic;
using System.Collections;

namespace Snowflake.Controller
{
    /// <summary>
    /// Stores a database of gamepad abstractions
    /// </summary>
    public interface IGamepadAbstractionStore
    {
        /// <summary>
        /// Gets a gamepad abstraction given a device name
        /// </summary>
        /// <param name="deviceName">The name of the device</param>
        /// <returns></returns>
        IGamepadAbstraction GetGamepadAbstraction(string deviceName);

        /// <summary>
        /// Sets an abstraction for a device name
        /// </summary>
        /// <param name="deviceName">The name of the device to set</param>
        /// <param name="gamepadAbstraction">The abstraction to set</param>
        void SetGamepadAbstraction(string deviceName, IGamepadAbstraction gamepadAbstraction);

        /// <summary>
        /// Indexer accessor for gamepad abstractions
        /// </summary>
        /// <param name="deviceName">The device name to get the abstraction for</param>
        IGamepadAbstraction this[string deviceName] { get; set; }

        /// <summary>
        /// Deletes a gamepad abstraction
        /// </summary>
        /// <param name="deviceName">The device name to delete the abstraction for</param>
        void RemoveGamepadAbstraction(string deviceName);

        /// <summary>
        /// Gets all gamepad abstractions
        /// </summary>
        /// <returns>All installed gamepad abstractions</returns>
        IEnumerable<IGamepadAbstraction> GetAllGamepadAbstractions();
    }
}
