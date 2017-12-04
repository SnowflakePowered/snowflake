using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Plugin.Emulators.TestEmulator.Configuration
{
    [ConfigurationSection("test", "Test Options")]
    public interface ITestConfiguration : IConfigurationSection<ITestConfiguration>
    {
        [ConfigurationOption("integer_option", 99)]
        int IntegerOption { get; }
        [ConfigurationOption("double_option", 15d)]
        double DoubleOption { get; }
        [ConfigurationOption("boolean_option", true)]
        bool BooleanOption { get; }
        [ConfigurationOption("string_option", "Hello World!")]
        string StringOption { get; }
        [ConfigurationOption("enum_option", TestConfigurationEnum.TestTwo)]
        TestConfigurationEnum EnumOption { get; }
        
    }
}
