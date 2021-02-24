using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Tests;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationTarget("target")]
    [ConfigurationCollection]
    public partial interface
        MissingPathConfigurationCollection
    {
        [ConfigurationTargetMember("target")] MissingPathConfigurationSection PathConfiguration { get; }
    }

    [ConfigurationSection("NoPath", "NoPath")]
    public partial interface MissingPathConfigurationSection
    {
        [ConfigurationOption("BadPath", "/my/bad/path", PathType.Directory)]
        string BadPath { get; set; }
    }

}
