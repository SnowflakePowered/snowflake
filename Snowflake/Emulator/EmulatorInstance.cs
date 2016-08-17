using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Input;
using Snowflake.Platform;
using Snowflake.Records.Game;

namespace Snowflake.Emulator
{
 
    public abstract class EmulatorInstance : IEmulatorInstance
    {
        protected EmulatorInstance(
            IEmulatorAdapter emulatorAdapter,
            IGameRecord game,
            int saveSlot,
            IPlatformInfo platform, 
            IList<IEmulatedPort> controllerPorts,
            IDictionary<string, IConfigurationCollection>  configurationCollections)
        {
            this.EmulatorAdapter = emulatorAdapter;
            this.InstanceGuid = Guid.NewGuid();
            this.Game = game;
            this.InstanceMetadata = new Dictionary<string, string>();
            this.ControllerPorts = controllerPorts;
            this.SaveSlot = saveSlot;
            this.ConfigurationCollections = configurationCollections;
            this.Platform = platform;
            string roamingAppdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            this.InstancePath = Path.Combine(roamingAppdata, "snowflake-cache", this.InstanceGuid.ToString());
            Directory.CreateDirectory(this.InstancePath);
        }

        
        public Guid InstanceGuid { get; }
        public IEmulatorAdapter EmulatorAdapter { get; }
        public IList<IEmulatedPort> ControllerPorts { get; }
        public int SaveSlot { get; }
        public IDictionary<string, IConfigurationCollection> ConfigurationCollections { get; }
        public IDictionary<string, string> InstanceMetadata { get; }
        public IGameRecord Game { get; protected set; }
        public IPlatformInfo Platform { get; }
        public string InstancePath { get; protected set; }
        public abstract void Create();
        public abstract void Start();
        public abstract void Pause();
        public abstract void Resume();
        public abstract void Destroy();
        public abstract DateTime StartTime { get; }
        public abstract DateTime DestroyTime { get; }
        public bool IsActive { get; protected set; }
        public bool IsRunning { get; protected set; }
        public bool IsGenerated { get; protected set; }
        public bool IsDestroyed { get; protected set; }
    }
}
