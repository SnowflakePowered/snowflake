using Snowflake.Configuration;
using Snowflake.Configuration.Generators;
using Snowflake.Configuration.Input;
using Snowflake.Configuration.Tests;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Services;
using System;
using Xunit;
using Snowflake.Generators.Configuration.Analyzers;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<Snowflake.Generators.Configuration.Analyzers.GenericArgumentRequiresConfigurationCollectionAnalyzer>;
using Snowflake.Configuration.Internal;
using System.Threading.Tasks;

namespace Snowflake.Framework.Tests.Configuration
{

    public class UnitTest1 
    {
        [Fact]
        public async Task TestUseAnalyzer()
        {
            string testCode = @"
namespace Snowflake.Framework.Tests.Configuration
{        
        public interface SomeInterface
        {
        }

        public class TestFixture
        {
            [GenericTypeAcceptsConfigurationCollection(0)]
            public void Use<T>()
                where T: class, IConfigurationCollectionTemplate
            {

            }

            public void Else()
            {
                this.Use<SomeInterface>();
            }
        }
}
";
            var expected = Verify.Diagnostic().WithLocation(1, 7);
            await Verify.VerifyAnalyzerAsync(testCode, expected);
        }


        [Fact]
        public void Test1()
        {
            var x = new ConfigurationSection<MyConfiguration>();
            var y = new ConfigurationCollection<ExampleConfigurationCollection>();

            y.Configuration.Sections.MyBoolean = true;
            var b = y.Configuration.GetValueDictionary()["Sections"].Descriptor;
            x.Configuration.MyBoolean = true;
            var mapcol = new ControllerElementMappingProfile("Keyboard",
                         "TEST_CONTROLLER",
                         InputDriver.Keyboard,
                         IDeviceEnumerator.VirtualVendorID,
                         new XInputDeviceInstance(0).DefaultLayout);

            IInputConfiguration<IRetroArchInput> z = new InputConfiguration<IRetroArchInput>(mapcol, 0);
            
            z[ControllerElement.ButtonA] = DeviceCapability.Button120;

            z.Configuration.InputDevice = 15;
        }
    }
}
