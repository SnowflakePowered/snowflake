using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Platform;
using Snowflake.Records.File;
using Snowflake.Records.Game;

namespace Snowflake.Emulator
{
    /// <summary>
    /// While the <see cref="IEmulatorAdapter"/> is responsible for spawning an instance, 
    /// the emulator instance does the heavy work. It is responsible for serializing valid configuration, 
    /// and managing the state of the emulator process. While not all emulators will support certain
    /// states, it must at the minimal implement <see cref="Create"/>, <see cref="Start"/>, and <see cref="Destroy"/>.
    /// as well as clean up after itself in the event of an unexpected occurance.
    /// </summary>
    public interface IEmulatorInstance
    {
        /// <summary>
        /// The unique GUID of this instance.
        /// </summary>
        Guid InstanceGuid { get; }
        /// <summary>
        /// The adapter associated with this instance
        /// </summary>
        IEmulatorAdapter EmulatorAdapter { get; }
        /// <summary>
        /// The list of emulated ports associated with this instance
        /// </summary>
        IList<IEmulatedPort> ControllerPorts { get; }
        /// <summary>
        /// The save slot used for save files within this instance.
        /// </summary>
        int SaveSlot { get; }
        /// <summary>
        /// The set of configuration collections for this instance
        /// </summary>
        IConfigurationCollection Configuration { get; }
        /// <summary>
        /// Various metadata about this instance
        /// </summary>
        IDictionary<string, string> InstanceMetadata { get; }
        /// <summary>
        /// The game this instance will launch
        /// </summary>
        IGameRecord Game { get; }
        /// <summary>
        /// The entrypoint file for the game, for this emulation.
        /// </summary>
        IFileRecord RomFile { get; }
        /// <summary>
        /// The platform being emulated.
        /// </summary>
        IPlatformInfo Platform { get; }
        /// <summary>
        /// The directory to store temporary instance files, such as generate configuration.
        /// </summary>
        string InstancePath { get; }
        /// <summary>
        /// The time this instance was created.
        /// </summary>
        DateTimeOffset CreateTime { get; }
        /// <summary>
        /// The time this instance was started
        /// </summary>
        DateTimeOffset StartTime { get; }
        /// <summary>
        /// The time this instance was destroyed.
        /// </summary>
        DateTimeOffset DestroyTime { get; }
        /// <summary>
        /// If this instance is active, it has been created and the process has been started.
        /// There are no guarantees the game is actually running, and it could be in a paused or suspended state.
        /// </summary>
        bool IsActive { get; }
        /// <summary>
        /// If this instance is running, the player is currently interfacing with the emulator, playing
        /// the game in an unpaused state.
        /// </summary>
        bool IsRunning { get; }
        /// <summary>
        /// If this instance is created, all prepatory work such as configuration generation has been completed and the
        /// game is ready to be started.
        /// </summary>
        bool IsCreated { get; }
        /// <summary>
        /// If this instance is destroyed, it has become stale and is no longer retrievable. A new instance must be 
        /// requested from the emulator adapter, and can be safely disposed. 
        /// </summary>
        bool IsDestroyed { get; }
        /// <summary>
        /// Creates the emulator instance, doing prepatory work such as generation and modification of
        /// configuration options.
        /// </summary>
        void Create();
        /// <summary>
        /// Starts the emulation, starting the process or similar in order to launch the emulator
        /// with the specified file.
        /// </summary>
        void Start();
        /// <summary>
        /// Pauses the emulation if possible. If the game is paused, 
        /// <see cref="IsActive"/> must be true, and <see cref="IsRunning"/> must be false.
        /// </summary>
        void Pause();
        /// <summary>
        /// Resumes the emulation if the game is paused, i.e. <see cref="IsActive"/> is true, and <see cref="IsRunning"/> is false.
        /// </summary>
        void Resume();
        /// <summary>
        /// Irreversibly destroys the instance, closing the emulator process cleanly, and freeing resources used by the instance.
        /// </summary>
        void Destroy();


    }
}