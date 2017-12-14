using System;
using System.Collections.Generic;
using System.IO;
using Snowflake.Configuration;
using Snowflake.Platform;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Utility;
namespace Snowflake.EmulatorOld
{
    public abstract class EmulatorInstance : IEmulatorInstance
    {
        protected EmulatorInstance(
            IEmulatorAdapter emulatorAdapter,
            IGameRecord game,
            IFileRecord file,
            int saveSlot,
            IPlatformInfo platform,
            IList<IEmulatedPort> controllerPorts)
        {
            this.EmulatorAdapter = emulatorAdapter;
            this.InstanceGuid = Guid.NewGuid();
            this.Game = game;
            this.RomFile = file;
            this.InstanceMetadata = new Dictionary<string, string>();
            this.ControllerPorts = controllerPorts;
            this.SaveSlot = saveSlot;
            this.Configuration = this.EmulatorAdapter.GetConfiguration(game);
            this.Platform = platform;
            this.InstancePath = PathUtility.GetSnowflakeDataPath("snowflake-cache", this.InstanceGuid.ToString());
            Directory.CreateDirectory(this.InstancePath);
        }

        /// <inheritdoc/>
        public Guid InstanceGuid { get; }

        /// <inheritdoc/>
        public IEmulatorAdapter EmulatorAdapter { get; }

        /// <inheritdoc/>
        public IList<IEmulatedPort> ControllerPorts { get; }

        /// <inheritdoc/>
        public int SaveSlot { get; }

        /// <inheritdoc/>
        public IConfigurationCollection Configuration { get; }

        /// <inheritdoc/>
        public IDictionary<string, string> InstanceMetadata { get; }

        /// <inheritdoc/>
        public IGameRecord Game { get; }

        /// <inheritdoc/>
        public IFileRecord RomFile { get; }

        /// <inheritdoc/>
        public IPlatformInfo Platform { get; }

        /// <inheritdoc/>
        public string InstancePath { get; }

        /// <inheritdoc/>
        public abstract void Create();

        /// <inheritdoc/>
        public abstract void Start();

        /// <inheritdoc/>
        public abstract void Pause();

        /// <inheritdoc/>
        public abstract void Resume();

        /// <inheritdoc/>
        public abstract void Destroy();

        /// <inheritdoc/>
        public abstract DateTimeOffset CreateTime { get; protected set; }

        /// <inheritdoc/>
        public abstract DateTimeOffset StartTime { get; protected set; }

        /// <inheritdoc/>
        public abstract DateTimeOffset DestroyTime { get; protected set; }

        /// <inheritdoc/>
        public bool IsActive { get; protected set; }

        /// <inheritdoc/>
        public bool IsRunning { get; protected set; }

        /// <inheritdoc/>
        public bool IsCreated { get; protected set; }

        /// <inheritdoc/>
        public bool IsDestroyed { get; protected set; }
    }
}
