using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Snowflake.JsonConverters;

namespace Snowflake.Emulator
{
    [JsonConverter(typeof(EmulatorAssemblyConverter))]
    public class EmulatorAssembly : IEmulatorAssembly
    {
        public string MainAssembly { get; }
        public string EmulatorID { get; }
        public string EmulatorName { get; }
        public EmulatorAssemblyType AssemblyType { get; }

        public EmulatorAssembly(string mainAssembly, string emulatorId, string name, EmulatorAssemblyType assemblyType)
        {
            this.MainAssembly = mainAssembly;
            this.EmulatorID = emulatorId;
            this.EmulatorName = name;
            this.AssemblyType = assemblyType;
        }
    }
}
