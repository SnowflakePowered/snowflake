using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters.Nestopia.Configuration
{
    public class NestopiaCoreConfigurationCollection : ConfigurationCollection
    {
        public NestopiaCoreConfiguration NestopiaCoreConfig { get; set; }

        public NestopiaCoreConfigurationCollection()
            : base(
                new KeyValuePairConfigurationSerializer(new BooleanMapping("enabled", "disabled"), "nul", "="),
                "nestopia-core-options.cfg")
        {
            
        }
    }
}
