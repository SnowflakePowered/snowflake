namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;

    public class SomeClass
    {
        [ConfigurationSection("TestInterface", "TestInterface")]
        public interface TestInterface
        {
        }
    }
}