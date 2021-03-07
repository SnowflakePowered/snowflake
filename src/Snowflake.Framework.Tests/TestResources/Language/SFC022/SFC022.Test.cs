namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration.Internal;

    public interface TestInterface
    {
    }

    public class TestFixture
    {
        [GenericTypeAcceptsConfigurationSection(0)]
        public void Use<T>()
            where T : class
        {
        }
        public void Else()
        {
            this.Use<TestInterface>();
        }
    }
}
