using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using Snowflake.Emulator;
using Snowflake.Scraper;
using Snowflake.Service.Manager;
using Xunit;

namespace Snowflake.Platform.Tests
{
    public class PlatformPreferencesDatabaseTests
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
            IReadOnlyDictionary<string, IEmulatorBridge> loadedEmulators = new Dictionary<string, IEmulatorBridge>
            {
                {"FakeEmulator", fakeEmulatorBridge.Object}
            };
            IReadOnlyDictionary<string, IScraper> loadedScrapers = new Dictionary<string, IScraper>
            {
                {"FakeScraper", fakeScraper.Object}
            };

            fakePluginManager.SetupGet(manager => manager.LoadedEmulators).Returns(loadedEmulators);
            fakePluginManager.SetupGet(manager => manager.LoadedScrapers).Returns(loadedScrapers);

            IPlatformPreferenceDatabase database = new PlatformPreferencesDatabase(filename, fakePluginManager.Object);

            Assert.NotNull(database);
            this.DisposeSqlite();
            File.Delete(filename);
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
            IReadOnlyDictionary<string, IEmulatorBridge> loadedEmulators = new Dictionary<string, IEmulatorBridge>
            {
                {"FakeEmulator", fakeEmulatorBridge.Object}
            };
            IReadOnlyDictionary<string, IScraper> loadedScrapers = new Dictionary<string, IScraper>
            {
                {"FakeScraper", fakeScraper.Object}
            };

            fakePluginManager.SetupGet(manager => manager.LoadedEmulators).Returns(loadedEmulators);
            fakePluginManager.SetupGet(manager => manager.LoadedScrapers).Returns(loadedScrapers);

            fakePlatform.SetupGet(platform => platform.PlatformID).Returns("TESTPLATFORM");
            IPlatformPreferenceDatabase database = new PlatformPreferencesDatabase(filename, fakePluginManager.Object);

            database.AddPlatform(fakePlatform.Object);
            
            var expectedPrefs = new PlatformDefaults("FakeScraper", "FakeEmulator");

            Assert.Equal(expectedPrefs.Emulator, database.GetPreferences(fakePlatform.Object).Emulator);
            Assert.Equal(expectedPrefs.Scraper, database.GetPreferences(fakePlatform.Object).Scraper);

            this.DisposeSqlite();
            File.Delete(filename);
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
            IReadOnlyDictionary<string, IEmulatorBridge> loadedEmulators = new Dictionary<string, IEmulatorBridge>
            {
                {"FakeEmulator", fakeEmulatorBridge.Object}
            };
            IReadOnlyDictionary<string, IScraper> loadedScrapers = new Dictionary<string, IScraper>
            {
                {"FakeScraper", fakeScraper.Object}
            };

            fakePluginManager.SetupGet(manager => manager.LoadedEmulators).Returns(loadedEmulators);
            fakePluginManager.SetupGet(manager => manager.LoadedScrapers).Returns(loadedScrapers);

            fakePlatform.SetupGet(platform => platform.PlatformID).Returns("TESTPLATFORM");
            IPlatformPreferenceDatabase database = new PlatformPreferencesDatabase(filename, fakePluginManager.Object);

            database.AddPlatform(fakePlatform.Object);

            database.SetEmulator(fakePlatform.Object, "TESTEMULATOR~~");
            Assert.Equal("TESTEMULATOR~~", database.GetPreferences(fakePlatform.Object).Emulator);

            this.DisposeSqlite();
            File.Delete(filename);
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
            IReadOnlyDictionary<string, IEmulatorBridge> loadedEmulators = new Dictionary<string, IEmulatorBridge>
            {
                {"FakeEmulator", fakeEmulatorBridge.Object}
            };
            IReadOnlyDictionary<string, IScraper> loadedScrapers = new Dictionary<string, IScraper>
            {
                {"FakeScraper", fakeScraper.Object}
            };

            fakePluginManager.SetupGet(manager => manager.LoadedEmulators).Returns(loadedEmulators);
            fakePluginManager.SetupGet(manager => manager.LoadedScrapers).Returns(loadedScrapers);

            fakePlatform.SetupGet(platform => platform.PlatformID).Returns("TESTPLATFORM");
            IPlatformPreferenceDatabase database = new PlatformPreferencesDatabase(filename, fakePluginManager.Object);

            database.AddPlatform(fakePlatform.Object);

            database.SetScraper(fakePlatform.Object, "TESTSCRAPER~~");
            Assert.Equal("TESTSCRAPER~~", database.GetPreferences(fakePlatform.Object).Scraper);

            this.DisposeSqlite();
            File.Delete(filename);
        }
        private void DisposeSqlite()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
