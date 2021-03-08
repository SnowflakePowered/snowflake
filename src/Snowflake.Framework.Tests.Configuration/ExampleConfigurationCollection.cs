using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Serialization.Serializers.Implementations;
using Snowflake.Framework.Tests.Configuration;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationTarget("#dolphin")]
    [ConfigurationTarget("#regularroot")]
    [ConfigurationTarget("TestNestedSection", "#dolphin")]
    [ConfigurationTarget("TestNestedNestedSection", "TestNestedSection")]

    [ConfigurationTarget("TestCycle1", "TestCycle2")]
    [ConfigurationTarget("TestCycle2", "TestCycle3")]
    [ConfigurationTarget("TestCycle3", "TestCycle1")]

    [ConfigurationCollection]
    public partial interface ExampleConfigurationCollection
    {
        [ConfigurationTargetMember("#dolphin")]
        MyConfiguration Sections { get; }

        MyOtherConfiguration OtherSections { get; }
    }
}
