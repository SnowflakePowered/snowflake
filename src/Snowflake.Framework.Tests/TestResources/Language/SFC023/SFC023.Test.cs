namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration.Internal;

    public interface TestInterface
        : IInputConfigurationTemplate
    {
    }

    public class TestFixture
    {
        [GenericTypeAcceptsInputConfiguration(0)]
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
