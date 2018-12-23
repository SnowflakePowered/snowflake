using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Configuration.Tests
{
    public class ConfigurationCollectionTests
    {
        [Fact]
        public void ValueInitializationTests()
        {
            var values = new List<(string, string, IConfigurationValue)>()
            {
                ("ExampleConfiguration", "ISOPath0", new ConfigurationValue("Test", Guid.Empty))
                
            };
            var configuration = new ConfigurationCollection<ExampleConfigurationCollection>(new ConfigurationValueCollection(values));
            Assert.Equal("Test", configuration.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(Guid.Empty, configuration.Configuration.ExampleConfiguration.Values["ISOPath0"].Guid);
        }

        [Fact]
        public void StringInitializationTests()
        {
            var values = new List<(string, string, (string, Guid))>()
            {
                ("ExampleConfiguration", "ISOPath0", ("Test", Guid.Empty)),
                ("ExampleConfiguration", "FullscreenResolution", (FullscreenResolution.Resolution1024X600.ToString(), Guid.Empty))
            };

            var collection = ConfigurationValueCollection.MakeExistingValueCollection<ExampleConfigurationCollection>(values, Guid.Empty);
            var configuration = new ConfigurationCollection<ExampleConfigurationCollection>(collection);
            Assert.Equal("Test", configuration.Configuration.ExampleConfiguration.ISOPath0);
            Assert.Equal(FullscreenResolution.Resolution1024X600, configuration.Configuration.ExampleConfiguration.FullscreenResolution);
            Assert.Equal(Guid.Empty, configuration.Configuration.ExampleConfiguration.Values["ISOPath0"].Guid);
            Assert.Equal(Guid.Empty, configuration.Configuration.ExampleConfiguration.Values["FullscreenResolution"].Guid);
        }

        [Fact]
        public void DefaultsTests()
        {
            var configuration = new ConfigurationCollection<ExampleConfigurationCollection>();
            Assert.Equal(configuration.Configuration.ExampleConfiguration.Descriptor["FullscreenResolution"].Default,
                configuration.Configuration.ExampleConfiguration.FullscreenResolution);
        }

        [Fact]
        public void DescriptorTests()
        {
            var configuration = new ConfigurationCollection<ExampleConfigurationCollection>();
            Assert.Equal("Display", configuration.Configuration.ExampleConfiguration.Descriptor.SectionName);
        }

        [Fact]
        public void NestedTest()
        {
            var configuration = new ConfigurationCollection<ExampleConfigurationCollection>();
            Assert.Equal(configuration.Configuration.ExampleConfiguration.Descriptor["FullscreenResolution"].Default,
                configuration.Configuration.ExampleConfiguration.Configuration.Configuration.FullscreenResolution);
        }

        [Fact]
        public void OrderTest()
        {
            var configuration = new ConfigurationCollection<OrderSensitiveConfigurationCollection>();
            var enumerator = configuration.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal("Display", enumerator.Current.Value.Descriptor.SectionName);
            enumerator.MoveNext();
            Assert.Equal("video", enumerator.Current.Value.Descriptor.SectionName);
        }
    }
}
