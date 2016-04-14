using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using Snowflake.Emulator.Configuration;
using Snowflake.Game;
using Xunit;

namespace Snowflake.Emulator.Tests
{
    public class ConfigurationFlagStoreTests
    {
        [Fact]
        public void CreateDatabase_Test()
        {
            string filename = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(filename);
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            fakeEmulatorBridge.SetupGet(bridge => bridge.PluginName).Returns("TEST_EMULATOR");
            fakeEmulatorBridge.SetupGet(bridge => bridge.PluginDataPath).Returns(filename);
            fakeEmulatorBridge.SetupGet(bridge => bridge.ConfigurationFlags).Returns(new Dictionary<string, IConfigurationFlag>());
            IConfigurationFlagStore flagStore = new ConfigurationFlagStore(fakeEmulatorBridge.Object);
            Assert.NotNull(flagStore);
            this.DisposeSqlite();
            Directory.Delete(filename, true);
        }
        [Fact]
        public void GetDefaultValue_Test()
        {
            string filename = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(filename);
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            fakeEmulatorBridge.SetupGet(bridge => bridge.PluginName).Returns("TEST_EMULATOR");
            fakeEmulatorBridge.SetupGet(bridge => bridge.PluginDataPath).Returns(filename);
            var fakeConfigurationFlag = new Mock<IConfigurationFlag>();
            fakeConfigurationFlag.SetupGet(flag => flag.DefaultValue).Returns("true");
            fakeConfigurationFlag.SetupGet(flag => flag.Key).Returns("TESTKEY");
            fakeConfigurationFlag.SetupGet(flag => flag.Type).Returns(ConfigurationFlagTypes.BOOLEAN_FLAG);

            var dict = new Dictionary<string, IConfigurationFlag>
            {
                {"TESTKEY", fakeConfigurationFlag.Object}
            };
            fakeEmulatorBridge.SetupGet(bridge => bridge.ConfigurationFlags).Returns(dict);

            IConfigurationFlagStore flagStore = new ConfigurationFlagStore(fakeEmulatorBridge.Object);
            Assert.Equal(true, flagStore.GetDefaultValue("TESTKEY", ConfigurationFlagTypes.BOOLEAN_FLAG));
            this.DisposeSqlite();
            Directory.Delete(filename, true);
        }

        [Fact]
        public void SetDefaultValue_Test()
        {
            string filename = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(filename);
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            fakeEmulatorBridge.SetupGet(bridge => bridge.PluginName).Returns("TEST_EMULATOR");
            fakeEmulatorBridge.SetupGet(bridge => bridge.PluginDataPath).Returns(filename);
            var fakeConfigurationFlag = new Mock<IConfigurationFlag>();
            fakeConfigurationFlag.SetupGet(flag => flag.DefaultValue).Returns("true");
            fakeConfigurationFlag.SetupGet(flag => flag.Key).Returns("TESTKEY");
            fakeConfigurationFlag.SetupGet(flag => flag.Type).Returns(ConfigurationFlagTypes.BOOLEAN_FLAG);

            var dict = new Dictionary<string, IConfigurationFlag>
            {
                {"TESTKEY", fakeConfigurationFlag.Object}
            };
            fakeEmulatorBridge.SetupGet(bridge => bridge.ConfigurationFlags).Returns(dict);

            IConfigurationFlagStore flagStore = new ConfigurationFlagStore(fakeEmulatorBridge.Object);
            flagStore.SetDefaultValue("TESTKEY", false, ConfigurationFlagTypes.BOOLEAN_FLAG);
            Assert.Equal(false, flagStore.GetDefaultValue("TESTKEY", ConfigurationFlagTypes.BOOLEAN_FLAG));
            this.DisposeSqlite();
            Directory.Delete(filename, true);
        }

        [Fact]
        public void AddGame_Test()
        {
            string filename = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(filename);
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            fakeEmulatorBridge.SetupGet(bridge => bridge.PluginName).Returns("TEST_EMULATOR");
            fakeEmulatorBridge.SetupGet(bridge => bridge.PluginDataPath).Returns(filename);
            var fakeConfigurationFlag = new Mock<IConfigurationFlag>();
            fakeConfigurationFlag.SetupGet(flag => flag.DefaultValue).Returns("true");
            fakeConfigurationFlag.SetupGet(flag => flag.Key).Returns("TESTKEY");
            fakeConfigurationFlag.SetupGet(flag => flag.Type).Returns(ConfigurationFlagTypes.BOOLEAN_FLAG);

            var dict = new Dictionary<string, IConfigurationFlag>
            {
                {"TESTKEY", fakeConfigurationFlag.Object}
            };

            var flagValues = new Dictionary<string, string>
            {
                {"TESTKEY", "false"}
            };
            fakeEmulatorBridge.SetupGet(bridge => bridge.ConfigurationFlags).Returns(dict);

            IConfigurationFlagStore flagStore = new ConfigurationFlagStore(fakeEmulatorBridge.Object);

            var fakeGameInfo = new Mock<IGameInfo>();
            fakeGameInfo.SetupGet(game => game.Title).Returns("TestGame");
            fakeGameInfo.SetupGet(game => game.UUID).Returns("TESTGAME");
            fakeGameInfo.SetupGet(game => game.PlatformID).Returns("TESTPLATFORM");

            flagStore.AddGame(fakeGameInfo.Object, flagValues);
            bool value = flagStore.GetValue(fakeGameInfo.Object, "TESTKEY", ConfigurationFlagTypes.BOOLEAN_FLAG);
            Assert.Equal(false, value);
            this.DisposeSqlite();
            Directory.Delete(filename, true);
        }
        private void DisposeSqlite()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
