using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Plugin.Emulators.TestEmulator.Configuration
{
    [ConfigurationFile("#test", "test.cfg", "true", "false")]
    interface ITestConfigurationCollection : IConfigurationCollection<ITestConfigurationCollection>
    {
        [SerializableSection("#test")]
        ITestConfiguration TestConfiguration { get; }
    }
}
