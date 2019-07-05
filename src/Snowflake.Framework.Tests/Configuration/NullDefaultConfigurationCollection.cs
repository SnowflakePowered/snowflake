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
    public interface
        NullDefaultConfigurationCollection : IConfigurationCollection<NullDefaultConfigurationCollection>
    {
        NullDefaultConfigurationSection NullConfiguration { get; set; }
    }

    [ConfigurationSection("Null", "Null")]
    public interface NullDefaultConfigurationSection : IConfigurationSection<NullDefaultConfigurationSection>
    {
        [ConfigurationOption("NullDefault", null)]
        string NullDefault { get; set; }
    }

}
