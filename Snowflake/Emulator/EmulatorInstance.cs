using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Records.Game;

namespace Snowflake.Emulator
{
    public abstract class EmulatorInstance : IEmulatorInstance
    {
        protected EmulatorInstance()
        {
            this.InstanceGuid = Guid.NewGuid();
            this.InstanceMetadata = new Dictionary<string, string>();
            string roamingAppdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            this.InstancePath = Path.Combine(roamingAppdata, "snowflake-cache", this.InstanceGuid.ToString());
            Directory.CreateDirectory(this.InstancePath);
        }

        public Guid InstanceGuid { get; }
        public IDictionary<string, string> InstanceMetadata { get; }
        public bool IsActive { get; protected set; }
        public bool IsRunning { get; protected set; }
        public bool IsGenerated { get; protected set; }
        public bool IsDestroyed { get; protected set; }
        public IGameRecord RunningGame { get; protected set; }
        public string InstancePath { get; protected set; }
        public abstract IDictionary<string, IConfigurationCollection> ConfigurationCollection { get; }
        public abstract void Create();
        public abstract void Start();
        public abstract void Pause();
        public abstract void Resume();
        public abstract void Destroy();
        public abstract DateTime StartTime { get; }
        public abstract DateTime DestroyTime { get; }
    }
}
