namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;

    [ConfigurationSection("TestSection", "TestSection")]
    public partial interface TestInterface
    {
        [ConfigurationOption("testprop", TestEnum.EnumItemOne)]
        TestEnum TestProperty { get; init; }
    }

    public enum TestEnum
    {
        EnumItemOne
    }
}
