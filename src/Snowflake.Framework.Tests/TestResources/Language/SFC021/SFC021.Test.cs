namespace Snowflake.Framework.Tests.Configuration
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using Snowflake.Configuration;
    using Snowflake.Configuration.Internal;

    public interface TestInterface
        : IConfigurationCollectionTemplate
    {
    }

    public class ImplementingClass
        : TestInterface
    {
        public IReadOnlyDictionary<string, IConfigurationSection> GetValueDictionary() => new Dictionary<string, IConfigurationSection>();
    }

    [GenericTypeAcceptsConfigurationCollection(0)]
    public class AcceptsClass<T>
        where T : class
    {

    }

    public class TestFixture
    {
        [GenericTypeAcceptsConfigurationCollection(0)]
        public void Use<T>()
            where T : class
        {
        }

        [GenericTypeAcceptsConfigurationCollection(0)]
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