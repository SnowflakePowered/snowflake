using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Snowflake.Emulator;
using Snowflake.Extensions;

namespace Snowflake.Service.Manager
{
    public class EmulatorAssembliesManager : IEmulatorAssembliesManager
    {
        private readonly IDictionary<string, IEmulatorAssembly> emulatorAssemblies;
        public IReadOnlyDictionary<string, IEmulatorAssembly> EmulatorAssemblies => this.emulatorAssemblies.AsReadOnly();
        public string AssembliesLocation { get; }
        public EmulatorAssembliesManager(string assembliesLocation)
        {
            this.emulatorAssemblies = new Dictionary<string, IEmulatorAssembly>();
            this.AssembliesLocation = assembliesLocation;
        }

        public void LoadEmulatorAssemblies()
        {
            if (!Directory.Exists(this.AssembliesLocation)) Directory.CreateDirectory(this.AssembliesLocation);
            foreach (var emulatorCore in 
                from fileName in Directory.GetFiles(this.AssembliesLocation)
                where Path.GetExtension(fileName) == ".emulatordef"
                select JsonConvert.DeserializeObject<EmulatorAssembly>(fileName))
            {
                this.emulatorAssemblies.Add(emulatorCore.EmulatorID, emulatorCore);
            }
        }

        public string GetAssemblyDirectory(IEmulatorAssembly assembly){
            return Path.Combine(this.AssembliesLocation, assembly.EmulatorID);
        }
    }
}
