namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;

    [ConfigurationSection("TestSection", "TestSection")]
    public partial interface TestInterface
    {
        [ConfigurationOption("string", "string")]
        string TestProperty { get; set; }
    }
}
