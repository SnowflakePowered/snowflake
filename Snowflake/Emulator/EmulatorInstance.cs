using System;
using System.Collections.Generic;
using System.IO;
using Snowflake.Configuration;
using Snowflake.Platform;
using Snowflake.Records.File;
using Snowflake.Records.Game;

namespace Snowflake.Emulator
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
            string roamingAppdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            this.InstancePath = Path.Combine(roamingAppdata, "snowflake-cache", this.InstanceGuid.ToString());
            Directory.CreateDirectory(this.InstancePath);
        }

        public Guid InstanceGuid { get; }
        public IEmulatorAdapter EmulatorAdapter { get; }
        public IList<IEmulatedPort> ControllerPorts { get; }
        public int SaveSlot { get; }
        public IConfigurationCollection Configuration { get; }
        public IDictionary<string, string> InstanceMetadata { get; }
        public IGameRecord Game { get; }
        public IFileRecord RomFile { get; }
        public IPlatformInfo Platform { get; }
        public string InstancePath { get; }
        public abstract void Create();
        public abstract void Start();
        public abstract void Pause();
        public abstract void Resume();
        public abstract void Destroy();
        public abstract DateTimeOffset CreateTime { get; protected set; }
        public abstract DateTimeOffset StartTime { get; protected set; }
        public abstract DateTimeOffset DestroyTime { get; protected set; }
        public bool IsActive { get; protected set; }
        public bool IsRunning { get; protected set; }
        public bool IsCreated { get; protected set; }
        public bool IsDestroyed { get; protected set; }
    }
}
