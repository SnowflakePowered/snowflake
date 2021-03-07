namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration;
    using Snowflake.Configuration.Internal;

    public interface TestInterface
        : IConfigurationCollectionTemplate
    {
    }

    public class TestFixture
    {
        [GenericTypeAcceptsConfigurationCollection(0)]
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
