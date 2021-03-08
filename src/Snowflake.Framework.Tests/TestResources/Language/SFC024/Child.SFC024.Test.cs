namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;

    [ConfigurationCollection]
    [ConfigurationTarget("ChildTarget", "TestTarget")]
    public partial interface TestInterface
    {
    }
}
