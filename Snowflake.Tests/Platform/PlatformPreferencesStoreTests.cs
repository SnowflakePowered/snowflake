using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using Snowflake.Emulator;
using Snowflake.Scraper;
using Snowflake.Service.Manager;
using Snowflake.Utility;
using Xunit;

namespace Snowflake.Platform.Tests
{
    public class PlatformPreferencesStoreTests
    {
        [Fact]
        public void CreateDatabase_Test()
        {
            string filename = Path.GetTempFileName();
            var fakePluginManager = new Mock<IPluginManager>();
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            var fakeScraper = new Mock<IScraper>();

            IList<string> supportedPlatforms = new List<string>
            {
                "TESTPLATFORM"
            };

            fakeEmulatorBridge.SetupGet(bridge => bridge.PluginName).Returns("FakeEmulator");
            fakeEmulatorBridge.SetupGet(bridge => bridge.SupportedPlatforms).Returns(supportedPlatforms);
            fakeScraper.SetupGet(scraper => scraper.PluginName).Returns("FakeScraper");
            fakeScraper.SetupGet(scraper => scraper.SupportedPlatforms).Returns(supportedPlatforms);
            IDictionary<string, IEmulatorBridge> loadedEmulators = new Dictionary<string, IEmulatorBridge>
            {
                {"FakeEmulator", fakeEmulatorBridge.Object}
            };
            IDictionary<string, IScraper> loadedScrapers = new Dictionary<string, IScraper>
            {
                {"FakeScraper", fakeScraper.Object}
            };

            fakePluginManager.Setup(manager => manager.Plugins<IEmulatorBridge>()).Returns(loadedEmulators);
            fakePluginManager.Setup(manager => manager.Plugins<IScraper>()).Returns(loadedScrapers);

            IPlatformPreferenceStore database = new PlatformPreferencesStore(filename, fakePluginManager.Object);

            Assert.NotNull(database);
        }

        [Fact]
        public void AddPlatform_Test()
        {
            string filename = Path.GetTempFileName();
            var fakePluginManager = new Mock<IPluginManager>();
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            var fakeScraper = new Mock<IScraper>();
            var fakePlatform = new Mock<IPlatformInfo>();
            IList<string> supportedPlatforms = new List<string>
            {
                "TESTPLATFORM"
            };

            fakeEmulatorBridge.SetupGet(bridge => bridge.PluginName).Returns("FakeEmulator");
            fakeEmulatorBridge.SetupGet(bridge => bridge.SupportedPlatforms).Returns(supportedPlatforms);
            fakeScraper.SetupGet(scraper => scraper.PluginName).Returns("FakeScraper");
            fakeScraper.SetupGet(scraper => scraper.SupportedPlatforms).Returns(supportedPlatforms);
            IDictionary<string, IEmulatorBridge> loadedEmulators = new Dictionary<string, IEmulatorBridge>
            {
                {"FakeEmulator", fakeEmulatorBridge.Object}
            };
            IDictionary<string, IScraper> loadedScrapers = new Dictionary<string, IScraper>
            {
                {"FakeScraper", fakeScraper.Object}
            };

            fakePluginManager.Setup(manager => manager.Plugins<IEmulatorBridge>()).Returns(loadedEmulators);
            fakePluginManager.Setup(manager => manager.Plugins<IScraper>()).Returns(loadedScrapers);

            fakePlatform.Setup(platform => platform.PlatformID).Returns("TESTPLATFORM");
            IPlatformPreferenceStore database = new PlatformPreferencesStore(filename, fakePluginManager.Object);

            database.AddPlatform(fakePlatform.Object);
            
            var expectedPrefs = new PlatformDefaults("FakeScraper", "FakeEmulator");

            Assert.Equal(expectedPrefs.Emulator, database.GetPreferences(fakePlatform.Object).Emulator);
            Assert.Equal(expectedPrefs.Scraper, database.GetPreferences(fakePlatform.Object).Scraper);
        }

        [Fact]
        public void SetEmulator_Test()
        {
            string filename = Path.GetTempFileName();
            var fakePluginManager = new Mock<IPluginManager>();
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            var fakeScraper = new Mock<IScraper>();
            var fakePlatform = new Mock<IPlatformInfo>();
            IList<string> supportedPlatforms = new List<string>
            {
                "TESTPLATFORM"
            };

            fakeEmulatorBridge.SetupGet(bridge => bridge.PluginName).Returns("FakeEmulator");
            fakeEmulatorBridge.SetupGet(bridge => bridge.SupportedPlatforms).Returns(supportedPlatforms);
            fakeScraper.SetupGet(scraper => scraper.PluginName).Returns("FakeScraper");
            fakeScraper.SetupGet(scraper => scraper.SupportedPlatforms).Returns(supportedPlatforms);
            IDictionary<string, IEmulatorBridge> loadedEmulators = new Dictionary<string, IEmulatorBridge>
            {
                {"FakeEmulator", fakeEmulatorBridge.Object}
            };
            IDictionary<string, IScraper> loadedScrapers = new Dictionary<string, IScraper>
            {
                {"FakeScraper", fakeScraper.Object}
            };

            fakePluginManager.Setup(manager => manager.Plugins<IEmulatorBridge>()).Returns(loadedEmulators);
            fakePluginManager.Setup(manager => manager.Plugins<IScraper>()).Returns(loadedScrapers);

            fakePlatform.SetupGet(platform => platform.PlatformID).Returns("TESTPLATFORM");
            IPlatformPreferenceStore database = new PlatformPreferencesStore(filename, fakePluginManager.Object);

            database.AddPlatform(fakePlatform.Object);

            database.SetEmulator(fakePlatform.Object, "TESTEMULATOR~~");
            Assert.Equal("TESTEMULATOR~~", database.GetPreferences(fakePlatform.Object).Emulator);
        }

        [Fact]
        public void SetScraper_Test()
        {
            string filename = Path.GetTempFileName();
            var fakePluginManager = new Mock<IPluginManager>();
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            var fakeScraper = new Mock<IScraper>();
            var fakePlatform = new Mock<IPlatformInfo>();
            IList<string> supportedPlatforms = new List<string>
            {
                "TESTPLATFORM"
            };

            fakeEmulatorBridge.SetupGet(bridge => bridge.PluginName).Returns("FakeEmulator");
            fakeEmulatorBridge.SetupGet(bridge => bridge.SupportedPlatforms).Returns(supportedPlatforms);
            fakeScraper.SetupGet(scraper => scraper.PluginName).Returns("FakeScraper");
            fakeScraper.SetupGet(scraper => scraper.SupportedPlatforms).Returns(supportedPlatforms);
            IDictionary<string, IEmulatorBridge> loadedEmulators = new Dictionary<string, IEmulatorBridge>
            {
                {"FakeEmulator", fakeEmulatorBridge.Object}
            };
            IDictionary<string, IScraper> loadedScrapers = new Dictionary<string, IScraper>
            {
                {"FakeScraper", fakeScraper.Object}
            };

            fakePluginManager.Setup(manager => manager.Plugins<IEmulatorBridge>()).Returns(loadedEmulators);
            fakePluginManager.Setup(manager => manager.Plugins<IScraper>()).Returns(loadedScrapers);

            fakePlatform.SetupGet(platform => platform.PlatformID).Returns("TESTPLATFORM");
            IPlatformPreferenceStore database = new PlatformPreferencesStore(filename, fakePluginManager.Object);

            database.AddPlatform(fakePlatform.Object);

            database.SetScraper(fakePlatform.Object, "TESTSCRAPER~~");
            Assert.Equal("TESTSCRAPER~~", database.GetPreferences(fakePlatform.Object).Scraper);
        }

        [Fact]
        public void Dispose_Test()
        {

            string filename = Path.GetTempFileName();
            var fakePluginManager = new Mock<IPluginManager>();
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            var fakeScraper = new Mock<IScraper>();

            IList<string> supportedPlatforms = new List<string>
            {
                "TESTPLATFORM"
            };

            fakeEmulatorBridge.SetupGet(bridge => bridge.PluginName).Returns("FakeEmulator");
            fakeEmulatorBridge.SetupGet(bridge => bridge.SupportedPlatforms).Returns(supportedPlatforms);
            fakeScraper.SetupGet(scraper => scraper.PluginName).Returns("FakeScraper");
            fakeScraper.SetupGet(scraper => scraper.SupportedPlatforms).Returns(supportedPlatforms);
            IDictionary<string, IEmulatorBridge> loadedEmulators = new Dictionary<string, IEmulatorBridge>
            {
                {"FakeEmulator", fakeEmulatorBridge.Object}
            };
            IDictionary<string, IScraper> loadedScrapers = new Dictionary<string, IScraper>
            {
                {"FakeScraper", fakeScraper.Object}
            };

            fakePluginManager.Setup(manager => manager.Plugins<IEmulatorBridge>()).Returns(loadedEmulators);
            fakePluginManager.Setup(manager => manager.Plugins<IScraper>()).Returns(loadedScrapers);

            IPlatformPreferenceStore database = new PlatformPreferencesStore(filename, fakePluginManager.Object);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            File.Delete(filename);
        }
    }
}
