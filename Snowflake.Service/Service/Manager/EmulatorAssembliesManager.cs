using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensions;
using Snowflake.Emulator;
using System.IO;
using Newtonsoft.Json;

namespace Snowflake.Service.Manager
{
    public class EmulatorAssembliesManager : IEmulatorAssembliesManager
    {
        private IDictionary<string, IEmulatorAssembly> emulatorAssemblies;
        public IReadOnlyDictionary<string, IEmulatorAssembly> EmulatorAssemblies { get { return this.emulatorAssemblies.AsReadOnly(); } }
        public string AssembliesLocation { get; }
        public EmulatorAssembliesManager(string assembliesLocation)
        {
            this.emulatorAssemblies = new Dictionary<string, IEmulatorAssembly>();
            this.AssembliesLocation = assembliesLocation;
        }

        public void LoadEmulatorAssemblies()
        {
            if (!Directory.Exists(this.AssembliesLocation)) Directory.CreateDirectory(this.AssembliesLocation);
            foreach (string fileName in Directory.GetFiles(this.AssembliesLocation))
            {
                if (!(Path.GetExtension(fileName) == ".emulatordef")) continue;
                
                var emulatorCore = EmulatorAssembliesManager.ParseEmulatorAssembly(fileName);
                this.emulatorAssemblies.Add(emulatorCore.EmulatorID, emulatorCore);
            }
        }
        public string GetAssemblyDirectory(IEmulatorAssembly assembly){
            return Path.Combine(this.AssembliesLocation, assembly.EmulatorID);
        }
        public static IEmulatorAssembly ParseEmulatorAssembly(string emulatorCorePath)
        {
            var emulator = JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText(emulatorCorePath));
            return EmulatorAssembly.FromJsonProtoTemplate(emulator);
        }

       
    }
}
