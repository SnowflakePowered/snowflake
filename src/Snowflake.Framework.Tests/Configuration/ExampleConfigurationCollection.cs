using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationFile("#dolphin", "Dolphin.ini")]
    [ConfigurationTarget("#dolphin", typeof(object))]
    [ConfigurationTarget("#regularroot", typeof(int))]
    [ConfigurationTarget("TestNestedSection", "#dolphin")]
    [ConfigurationTarget("TestNestedNestedSection", "TestNestedSection")]

    [ConfigurationTarget("TestCycle1", "TestCycle2")]
    [ConfigurationTarget("TestCycle2", "TestCycle1")]
    public interface ExampleConfigurationCollection : IConfigurationCollection<ExampleConfigurationCollection>
    {
        [ConfigurationTargetMember("#dolphin")]
        ExampleConfigurationSection ExampleConfiguration { get; }
    }
}
