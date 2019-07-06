﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Serialization.Serializers.Implementations;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationTarget("#dolphin")]
    [ConfigurationTarget("#regularroot")]
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
