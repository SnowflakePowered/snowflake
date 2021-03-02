using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;

namespace Snowflake.Plugin.Emulators.RetroArch.Adapters.Higan.Configuration
{
    [ConfigurationTarget("#coreoptions")]
    [ConfigurationCollection]
    public partial interface HiganRetroArchConfiguration
        : RetroArchConfiguration
    {
        [ConfigurationTargetMember("#coreoptions")] BsnesCoreConfiguration BsnesCoreConfig { get; }
    }
}
