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

    public interface ExampleConfigurationCollection : IConfigurationCollection<ExampleConfigurationCollection>
    {
        [ConfigurationTargetMember("#dolphin")]
        ExampleConfigurationSection ExampleConfiguration { get; }
    }
}
