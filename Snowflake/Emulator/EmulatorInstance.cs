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
    public class EmulatorInstance : IEmulatorInstance
    {
        public Guid InstanceGuid { get; }
        public bool IsActive { get; }
        public bool IsRunning { get; }
        public bool IsGenerated { get; }
        public bool IsDestroyed { get; }
        public IEmulatorAssembly EmulatorAssembly { get; }
        public IGameRecord EmulatedGame { get; }
        public string InstanceDirectory { get; }
        public IDictionary<IConfigurationSection, IConfigurationSerializer> ConfigurationSections { get; }
        public IConfigurationSection ConfigurationFlagSection { get; }

        public void Create()
        {
           /*  var configs = (from config in this.ConfigurationSections
             select Tuple.Create(config.Key.ConfigurationFileName, config.Value.Serialize(config.Key)));

            foreach (var config in configs)
            {
                File.AppendAllText(config.Item1, config.Item2 + Environment.NewLine);
            }*/
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public DateTime StartTime { get; }
        public DateTime DestroyTime { get; }
    }
}
