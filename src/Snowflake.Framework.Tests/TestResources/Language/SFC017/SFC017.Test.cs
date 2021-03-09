namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;
    using Snowflake.Configuration.Input;

    [InputConfiguration("Test")]
    public partial interface TestCollection
    {
        string TestProperty { get; set; }
    }
}
