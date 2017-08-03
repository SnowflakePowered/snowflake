using Snowflake.Emulator;
using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Resources.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Resources.Emulators
{
    [Resource("emulators")]
    public class EmulatorsRoot : Resource
    {
        private IEnumerable<IEmulatorAdapter> Adapter { get; }
        public EmulatorsRoot(IEnumerable<IEmulatorAdapter> adapter)
        {
            this.Adapter = adapter;
        }

        [Endpoint(EndpointVerb.Read)]
        public IEnumerable<IEmulatorAdapter> GetAdapters()
        {
            return this.Adapter;
        }
    }
}
