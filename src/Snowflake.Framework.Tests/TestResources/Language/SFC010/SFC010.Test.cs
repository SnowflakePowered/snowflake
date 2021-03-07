namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;

    [ConfigurationSection("Test", "Test")]
    public partial interface TestCollection
    {
        string TestProperty { get; set; }
    }
}
