namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;

    [InputConfiguration("TestInterface")]
    public partial interface TestInterface
    {
        string this[string accessor] { get; }
    }
}
