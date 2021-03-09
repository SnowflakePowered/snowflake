namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;

    public interface TestSection
    {
    }

    [ConfigurationCollection]
    public interface TestCollection
    {
        TestSection Section { get; }
    }
}
