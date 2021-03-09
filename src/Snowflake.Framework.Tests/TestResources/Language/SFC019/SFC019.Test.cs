namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;
    using Snowflake.Configuration.Input;

    [InputConfiguration("Test")]
    [ConfigurationSection("Test", "Test")]
    public partial interface TestInterface
    {
    }
}
