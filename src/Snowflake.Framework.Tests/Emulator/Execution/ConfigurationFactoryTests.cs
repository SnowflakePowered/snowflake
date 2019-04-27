using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Newtonsoft.Json;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Configuration.Tests;
using Snowflake.Execution.Extensibility;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Tests;
using Xunit;

namespace Snowflake.Emulator.Execution.Tests
{
    public class ConfigurationFactoryTests
    {
        [Fact]
        public void ConfigurationFactoryCreation_Tests()
        {
            IInputMapping mapping = JsonConvert.DeserializeObject<InputMapping>
                (TestUtilities.GetStringResource("InputMappings.DirectInput.KEYBOARD_DEVICE.json"));

            var configFactory = new TestConfigurationFactory(new[] {mapping});
        }

        [Fact]
        public void ConfigurationFactoryConfiguration_Test()
        {
            IInputMapping mapping = JsonConvert.DeserializeObject<InputMapping>
                (TestUtilities.GetStringResource("InputMappings.DirectInput.KEYBOARD_DEVICE.json"));

            var configFactory = new TestConfigurationFactory(new[] {mapping});
            var config = configFactory.GetConfiguration();

            Assert.Equal(config.Configuration.ExampleConfiguration.FullscreenResolution,
                new ConfigurationCollection<ExampleConfigurationCollection>().Configuration.ExampleConfiguration
                    .FullscreenResolution);
            Assert.Equal(config.Configuration.ExampleConfiguration.FullscreenResolution,
                ((IConfigurationFactory) configFactory).GetConfiguration()["ExampleConfiguration"][
                    "FullscreenResolution"]);
        }

        [Fact]
        public void ConfigurationFactoryInput_Test()
        {
            IInputMapping mapping = JsonConvert.DeserializeObject<InputMapping>
                (TestUtilities.GetStringResource("InputMappings.DirectInput.KEYBOARD_DEVICE.json"));

            var configFactory = new TestConfigurationFactory(new[] {mapping});

            var realLayout =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.keyboard_device.json"));

            var targetLayout =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));

            var fakeInputDevice = new Mock<IInputDevice>();
            fakeInputDevice.Setup(p => p.DeviceId).Returns("KEYBOARD_DEVICE");
            fakeInputDevice.Setup(p => p.DeviceLayout).Returns(realLayout);
            fakeInputDevice.Setup(p => p.DeviceApi).Returns(InputApi.DirectInput);

            var map = ControllerElementMappings.GetDefaultMappings(realLayout, targetLayout);
            var emulatedController = new EmulatedController(0, fakeInputDevice.Object, targetLayout, map);

            (IInputTemplate template, IInputMapping inputMappings) = configFactory.GetInputMappings(emulatedController);
            Assert.Equal(ControllerElement.KeyZ, template[ControllerElement.ButtonA]);
        }
    }

    internal class TestConfigurationFactory : ConfigurationFactory<ExampleConfigurationCollection, IRetroArchInput>
    {
        public TestConfigurationFactory(IEnumerable<IInputMapping> inputMappings)
            : base(inputMappings)
        {
        }

        protected TestConfigurationFactory(IPluginProvision provision)
            : base(provision)
        {
            throw new NotImplementedException();
        }

        public override IConfigurationCollection<ExampleConfigurationCollection> GetConfiguration(Guid gameRecord,
            string profileName)
        {
            return new ConfigurationCollection<ExampleConfigurationCollection>();
        }

        public override IConfigurationCollection<ExampleConfigurationCollection> GetConfiguration()
        {
            return new ConfigurationCollection<ExampleConfigurationCollection>();
        }

        public override (IInputTemplate<IRetroArchInput> template, IInputMapping mapping) GetInputMappings(
            IEmulatedController emulatedDevice)
        {
            var template = new InputTemplate<IRetroArchInput>(emulatedDevice.LayoutMapping, emulatedDevice.PortIndex);
            var mapping = (from inputMappings in this.InputMappings
                where inputMappings.InputApi == InputApi.DirectInput
                where inputMappings.DeviceLayouts.Contains(emulatedDevice.PhysicalDevice.DeviceLayout.LayoutId.ToString())
                select inputMappings).FirstOrDefault();

            return (template, mapping);
        }
    }
}
