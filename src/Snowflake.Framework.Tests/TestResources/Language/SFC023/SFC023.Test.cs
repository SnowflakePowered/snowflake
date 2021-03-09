namespace Snowflake.Framework.Tests.Configuration
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using Snowflake.Configuration;
    using Snowflake.Configuration.Internal;
    using Snowflake.Input.Device;

    public interface TestInterface
        : IInputConfigurationTemplate
    {
    }

    public class ImplementingClass
        : TestInterface
    {
        public IReadOnlyDictionary<string, DeviceCapability> GetValueDictionary() => new Dictionary<string, DeviceCapability>();
        public DeviceCapability this[string keyName] { set { } }
    }

    [GenericTypeAcceptsInputConfiguration(0)]
    public class AcceptsClass<T>
        where T : class
    {

    }

    public class TestFixture
    {
        [GenericTypeAcceptsInputConfiguration(0)]
        public void Use<T>()
            where T : class
        {
        }

        [GenericTypeAcceptsInputConfiguration(0)]
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