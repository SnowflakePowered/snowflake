namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;

    [InputConfiguration("TestInterface")]
    public partial interface TestInterface
    {
        string InheritedProperty { get { return "no"; } }
    }
}
