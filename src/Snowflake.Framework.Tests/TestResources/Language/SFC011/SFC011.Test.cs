namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;

    [ConfigurationSection("Test", "Test")]
    public partial interface TestCollection
    {
        [ConfigurationOption("Test", 0)]
        string TestString { get; set; }

        [ConfigurationOption("Test", "")]
        double TestDouble { get; set; }

        [ConfigurationOption("Test", "")]
        int TestInt { get; set; }

        [ConfigurationOption("Test", "")]
        bool TestBool { get; set; }

        [ConfigurationOption("Test", "")]
        System.Guid TestGuid { get; set; }
    }
}
