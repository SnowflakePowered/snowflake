using Snowflake.Configuration;
using Snowflake.Emulator;
using Snowflake.Records.Game;
using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Resources.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Resources.Emulators
{
    [Resource("emulators", ":emulatorName", "config", ":game")]
    [Parameter(typeof(string), "emulatorName")]
    [Parameter(typeof(IGameRecord), "game")]
    public class EmulatorsGameConfigRoot : Resource
    {
        private IEnumerable<IEmulatorAdapter> Adapter { get; }
        public EmulatorsGameConfigRoot(IEnumerable<IEmulatorAdapter> adapter)
        {
            this.Adapter = adapter;
        }

        [Endpoint(EndpointVerb.Read)]
        public IConfigurationCollection GetConfig(string emulatorName, IGameRecord game)
        {
            var adapter = this.Adapter.FirstOrDefault(a => a.Name == emulatorName);
            return adapter?.GetConfiguration(game);
        }
    }
}
