namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;

    public interface TestInterface
    {
        string InheritedProperty { get; }
    }

    [InputConfiguration("TestInterface")]
    public partial interface TestInterfaceChild
        : TestInterface
    {
        string InheritedProperty { get; }
    }
}
