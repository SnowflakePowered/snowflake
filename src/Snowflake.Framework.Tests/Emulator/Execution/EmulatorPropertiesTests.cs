using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Snowflake.Execution.Extensibility;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Services;
using Xunit;

namespace Snowflake.Emulator.Execution
{
    public class EmulatorPropertiesTests
    {
        [Fact]
        public void EmulatorPropertiesCreation_Tests()
        {
            var stone = new StoneProvider();
            var pluginProps = new Mock<IPluginProperties>();
            pluginProps.Setup(p => p.GetEnumerable("capabilities"))
                .Returns(new[] {"testcapability", "testcapability2"});
            pluginProps.Setup(p => p.GetEnumerable("mimetypes"))
                .Returns(new[] {"application/vnd.stone-romfile.nintendo.snes"});
            pluginProps.Setup(p => p.Get("saveformat"))
                .Returns("test-sram");
            pluginProps.Setup(p => p.GetEnumerable("optionalbios"))
                .Returns(new[] {"BS-X.bin"});
            pluginProps.Setup(p => p.GetEnumerable("requiredbios"))
                .Returns(new[] {"cx4.data.rom"});
            var pluginProvision = new Mock<IPluginProvision>();
            pluginProvision.Setup(p => p.Properties)
                .Returns(pluginProps.Object);

            IEmulatorProperties emulatorProps = new EmulatorProperties(pluginProvision.Object, stone);
            Assert.Contains("testcapability", emulatorProps.SpecialCapabilities);
            Assert.Contains("testcapability2", emulatorProps.SpecialCapabilities);
            Assert.Contains("application/vnd.stone-romfile.nintendo.snes", emulatorProps.Mimetypes);
            Assert.Equal("test-sram", emulatorProps.SaveFormat);
            Assert.Equal("fed4d8242cfbed61343d53d48432aced", emulatorProps.OptionalSystemFiles.First().Md5Hash);
            Assert.Equal("037ac4296b6b6a5c47c440188d3c72e3", emulatorProps.RequiredSystemFiles.First().Md5Hash);
        }
    }
}
