using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Game;

namespace Snowflake.Emulator
{
    /// <summary>
    /// Represents an instance of an emulator. This can be an external process or a called DLL. 
    /// This object is usually created by a bridge plugin. Setup of the instance is the responsibiltity of the bridge plugin.
    /// </summary>
    public interface IEmulatorInstance
    {

        /// <summary>
        /// The current state of the instance
        /// </summary>
        EmulatorInstanceState InstanceState { get; }
        /// <summary>
        /// The game this instance is associated with.
        /// </summary>
        IGameInfo InstanceGame { get; }
        /// <summary>
        /// A temporary directory created for this instance. This directory should be deleted if the state ever reaches "InstanceClosed"
        /// </summary>
        string InstanceTemporaryDirectory { get; }
        /// <summary>
        /// The emulator bridge this instance is associated with.
        /// </summary>
        IEmulatorBridge InstanceEmulator { get; }
        /// <summary>
        /// Start the game assigned to this instance
        /// </summary>
        /// <returns>The resultant state of the action</returns>
        EmulatorInstanceState StartGame();
        /// <summary>
        /// Pause the game currently running in the instance
        /// </summary>
        /// <returns>The resultant state of the action</returns>
        EmulatorInstanceState PauseGame();
        /// <summary>
        /// Shutdown the game currently running in the instance
        /// </summary>
        /// <returns>The resultant state of the action</returns>
        EmulatorInstanceState ShutdownGame();
        /// <summary>
        /// Closes and cleans up the instance.
        /// </summary>
        /// <returns>The resultant state of the action</returns>
        EmulatorInstanceState CleanupInstance();
        /// <summary>
        /// This method can be overridden by plugins to handle sending custom messages, and optionally receive a response
        /// </summary>
        /// <returns>The resultant state of the action</returns>
        EmulatorInstanceState SendCustomMessage(string message, out string response);
        /// <summary>
        /// A unique identifier of the instance
        /// </summary>
        string InstanceId { get; }

    }
}
