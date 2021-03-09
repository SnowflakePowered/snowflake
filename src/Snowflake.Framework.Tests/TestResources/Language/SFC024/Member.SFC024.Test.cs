namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;

    [ConfigurationCollection]
    public partial interface TestInterface
    {
        [ConfigurationTargetMember("TestTarget")]
        string TestProperty { get; }
    }
}
