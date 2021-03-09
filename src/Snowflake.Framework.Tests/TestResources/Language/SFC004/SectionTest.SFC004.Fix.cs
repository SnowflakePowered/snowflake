namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;

    public interface TestInterface
    {
        string InheritedProperty { get; }
    }

    [ConfigurationSection("TestInterface", "TestInterface")]
    public partial interface TestInterfaceChild
        : TestInterface
    {
    }
}
