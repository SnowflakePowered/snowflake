using Snowflake.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    [ConfigurationTarget("#cemu")]
    public partial interface CemuConfigurationCollection : IConfigurationCollection<CemuConfigurationCollection>
    {
        [ConfigurationTargetMember("#cemu", true)]
        CemuRootConfigSection Content { get; set; }

        [ConfigurationTargetMember("#cemu")]
        GameListConfig Style { get; set; }
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
