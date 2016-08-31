using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters.bsnes.Configuration
{
    public class BsnesConfigurationCollection : ConfigurationCollection
    {
        public BsnesConfiguration BsnesCoreConfig { get; set; }

        public BsnesConfigurationCollection()
            : base(
                new KeyValuePairConfigurationSerializer(new BooleanMapping("enabled", "disabled"), "nul", "="),
                "bsnes-core-options.cfg")
        {
      
        }
    }
}
