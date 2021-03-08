namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;

    [ConfigurationCollection]
    [ConfigurationTarget("TestTarget")]
    public partial interface TestInterface
    {
        [ConfigurationTargetMember("TestTarget")]
        string TestProperty { get; }
    }
}
