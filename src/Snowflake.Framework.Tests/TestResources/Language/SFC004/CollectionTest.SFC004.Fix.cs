namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;

    public interface TestInterface
    {
        string InheritedProperty { get; }
    }

    [ConfigurationCollection]
    public partial interface TestInterfaceChild
        : TestInterface
    {
    }
}
