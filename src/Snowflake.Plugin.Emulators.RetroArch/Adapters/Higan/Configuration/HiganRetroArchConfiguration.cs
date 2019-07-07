using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.Emulators.RetroArch.Adapters.Higan.Configuration
{
    [ConfigurationTarget("#coreoptions")]
    public interface HiganRetroArchConfiguration : RetroArchConfiguration,
        IConfigurationCollection<HiganRetroArchConfiguration>
    {
        [ConfigurationTargetMember("#coreoptions")] BsnesCoreConfiguration BsnesCoreConfig { get; set; }
    }
}
