namespace Snowflake.Framework.Tests.Configuration
{
    using Snowflake.Configuration.Internal;

    public interface TestInterface
    {
    }

    public class ImplementingClass
        : TestInterface
    {

    }

    [GenericTypeAcceptsConfigurationSection(0)]
    public class AcceptsClass<T>
        where T : class
    {

    }

    public class TestFixture
    {
        [GenericTypeAcceptsConfigurationSection(0)]
        public void Use<T>()
            where T : class
        {
        }

        [GenericTypeAcceptsConfigurationSection(0)]
        public void Use<T>(T configuration)
           where T : class
        {
        }


        public void Else()
        {
            this.Use<TestInterface>();
            var configSection = new AcceptsClass<TestInterface>();
            AcceptsClass<TestInterface> testClass = new();
            this.Use(new ImplementingClass());
        }
    }
}