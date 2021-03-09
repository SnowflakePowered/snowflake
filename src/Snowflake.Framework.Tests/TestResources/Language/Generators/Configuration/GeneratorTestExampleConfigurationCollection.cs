namespace Snowflake.Language.Generators.Test
{
    using Snowflake.Configuration;

    [ConfigurationTarget("#dolphin")]
    [ConfigurationTarget("#regularroot")]
    [ConfigurationTarget("TestNestedSection", "#dolphin")]
    [ConfigurationTarget("TestNestedNestedSection", "TestNestedSection")]
    [ConfigurationTarget("TestNestedNestedNestedSection", "TestNestedNestedSection")]

    [ConfigurationCollection]
    public partial interface ExampleConfigurationCollection
    {
        [ConfigurationTargetMember("#dolphin")]
        ExampleConfigurationSection ExampleConfiguration { get; }

        [ConfigurationTargetMember("TestNestedSection")]
        ExampleConfigurationSection ExampleConfiguration2 { get; }
    }
}
