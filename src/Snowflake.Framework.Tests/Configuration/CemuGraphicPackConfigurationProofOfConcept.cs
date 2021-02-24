using Snowflake.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    [ConfigurationTarget("#cemu")]
    [ConfigurationCollection]
    public partial interface CemuConfigurationCollection
    {
        [ConfigurationTargetMember("#cemu", true)]
        CemuRootConfigSection Content { get; }

        [ConfigurationTargetMember("#cemu")]
        GameListConfig Style { get; }
    }

    [ConfigurationSection("content", "Main Config")]
    public partial interface CemuRootConfigSection
    {
        [ConfigurationOption("fullscreen", true)]
        bool Fullscreen { get; set; }
    }

    [ConfigurationSection("gamelist", "Gamelist Config")]
    public partial interface GameListConfig 
    {
        [ConfigurationOption("style", 0)]
        int Style { get; set; }
    }

}
