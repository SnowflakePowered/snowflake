using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Snowflake.Emulator;

namespace Snowflake.JsonConverters
{
    public class EmulatorAssemblyConverter : JsonCreationConverter<IEmulatorAssembly>
    {
        protected override IEmulatorAssembly Create(Type objectType, JObject jObject)
        {
            string assemblyMain = jObject.Value<string>("main");
            string assemblyId = jObject.Value<string>("id");
            string friendlyName = jObject.Value<string>("name");
            EmulatorAssemblyType assemblyType =
                (EmulatorAssemblyType)Enum.Parse(typeof (EmulatorAssemblyType), jObject.Value<string>("type"), true);
            return new EmulatorAssembly(assemblyMain, assemblyId, friendlyName, assemblyType);
        }
    }
}
