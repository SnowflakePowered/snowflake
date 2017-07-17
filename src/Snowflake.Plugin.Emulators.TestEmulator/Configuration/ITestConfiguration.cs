using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Plugin.Emulators.TestEmulator.Configuration
{
    [ConfigurationSection("audio", "Audio Options")]
    public interface ITestConfiguration : IConfigurationSection<ITestConfiguration>
    {
        [ConfigurationOption("integer_option", 0)]
        int IntegerOption { get; }
        [ConfigurationOption("double_option", 0d)]
        double DoubleOption { get; }
        [ConfigurationOption("boolean_option", false)]
        bool BooleanOption { get; }
        [ConfigurationOption("string_option", "Hello World!")]
        string StringOption { get; }
        [ConfigurationOption("enum_option", TestConfigurationEnum.TestOne)]
        TestConfigurationEnum EnumOption { get; }
        
    }
}
