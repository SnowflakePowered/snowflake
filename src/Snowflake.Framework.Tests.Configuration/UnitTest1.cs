using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Configuration.Tests;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Services;
using System;
using Xunit;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.XUnit
    .AnalyzerVerifier<Snowflake.Language.Analyzers.Configuration.GenericArgumentRequiresConfigurationSectionAnalyzer>;
using Snowflake.Configuration.Internal;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Snowflake.Language.Analyzers.Configuration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;

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
        using Snowflake.Configuration.Internal;

        public interface SomeInterface
        {
        }

        public class TestFixture
        {
            [GenericTypeAcceptsConfigurationSection(0)]
            public void Use<T>()
                where T: class
            {
            }
            public void Else()
            {
                this.Use<SomeInterface>();
            }
        }
}
";
            var verifier = new CSharpAnalyzerTest<GenericArgumentRequiresConfigurationSectionAnalyzer, XUnitVerifier>()
            {
                TestCode = testCode,
                ReferenceAssemblies = ReferenceAssemblies.Net.Net50,
                SolutionTransforms =
                {
                    DiagonosticVerifier.AddSnowflakeReferences,
                },
                TestState =
                {
                    ExpectedDiagnostics = { Verify.Diagnostic()
                        .WithArguments("SomeInterface")
                        .WithLocation(19, 17)
                    },
                },
            };

            await verifier.RunAsync();
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
