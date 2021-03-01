using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Tests;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationCollection]
    public partial interface
        NullDefaultConfigurationCollection
    {
        NullDefaultConfigurationSection NullConfiguration { get; }
    }

    [ConfigurationSection("Null", "Null")]
    public partial interface NullDefaultConfigurationSection
    {
        [ConfigurationOption("NullDefault", null, "UNSET")]
        string NullDefault { get; set; }
    }

}
