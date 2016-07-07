using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Records.Game;

namespace Snowflake.Emulator
{
    public interface IEmulatorInstance
    {
        /// <summary>
        /// The unique ID of this instance
        /// </summary>
        Guid InstanceGuid { get; }
        /// <summary>
        /// Whether or not this instance is active
        /// </summary>
        bool IsActive { get; }
        /// <summary>
        /// Whether or not this instance is running
        /// </summary>
        bool IsRunning { get; }
        /// <summary>
        /// Whether or not this instance has been generated
        /// </summary>
        bool IsGenerated { get; }
        /// <summary>
        /// Whether or not the instance has been destroyed
        /// </summary>
        bool IsDestroyed { get; }
        /// <summary>
        /// The emulator assembly associated with this instance
        /// </summary>
        IEmulatorAssembly EmulatorAssembly { get; }
        /// <summary>
        /// The emulated game associated with this instance
        /// </summary>
        IGameRecord EmulatedGame { get; }
        /// <summary>
        /// The temporary working directory of this instance, whether all configuration files are to be cached,
        /// as well as the instance itself.
        /// </summary>
        string InstanceDirectory { get; }
        /// <summary>
        /// The configuration sections associated with this instance
        /// </summary>
        IDictionary<IConfigurationSection, IConfigurationSerializer> ConfigurationSections { get; }
        /// <summary>
        /// todo configuration flags
        /// </summary>
        IConfigurationSection ConfigurationFlagSection { get; }
        /// <summary>
        /// Creates the instance and serializes all configuration nescessary.
        /// </summary>
        void Create();
        /// <summary>
        /// Starts the instance 
        /// </summary>
        void Start();
        /// <summary>
        /// Suspends the instance.
        /// </summary>
        void Pause();
        /// <summary>
        /// Stops and destroys the instance. After destruction the instance can no longer be recreated.
        /// </summary>
        void Destroy();
        /// <summary>
        /// Saves the instance. Could also save the game.
        /// </summary>
        void Save();
        /// <summary>
        /// The time this instance started
        /// </summary>
        DateTime StartTime { get; }
        /// <summary>
        /// The time this instance was destroyed
        /// </summary>
        DateTime DestroyTime { get; }


    }
}