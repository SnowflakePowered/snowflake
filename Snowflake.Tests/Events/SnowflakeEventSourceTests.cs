using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Events;
using Snowflake.Events.CoreEvents.GameEvent;
using Snowflake.Events.CoreEvents.ModifyEvent;
using Snowflake.Events.ServiceEvents;
using Snowflake.Tests.Fakes;
using Xunit;
namespace Snowflake.Events.Tests
{

    public class SnowflakeEventSourceTests
    {
        public SnowflakeEventSourceTests()
        {
            SnowflakeEventSource.InitEventSource();
        }

        [Fact]
        public void GameAddEvent_Test()
        {
            var args = new GameAddEventArgs(new FakeCoreService(), new FakeGameInfo(), new FakeGameDatabase());
            SnowflakeEventSource.EventSource.GameAdd += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameAdd(args);
        }
        [Fact]
        public void GameDeleteEvent_Test()
        {
            var args = new GameDeleteEventArgs(new FakeCoreService(), new FakeGameInfo(), new FakeGameDatabase());
            SnowflakeEventSource.EventSource.GameDelete += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameDelete(args);
        }
        [Fact]
        public void GameProcessQuitEventArgs_Test()
        {
            var args = new GameProcessQuitEventArgs(new FakeCoreService(), new FakeGameInfo(), new FakeEmulatorAssembly(), new FakeEmulatorBridge(), new System.Diagnostics.Process());
            SnowflakeEventSource.EventSource.GameProcessQuit += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameProcessQuit(args);
        }
        [Fact]
        public void GameProcessStartEventArgs_Test()
        {
            var args = new GameProcessStartEventArgs(new FakeCoreService(), new FakeGameInfo(), new FakeEmulatorAssembly(), new FakeEmulatorBridge(), new System.Diagnostics.Process());
            SnowflakeEventSource.EventSource.GameProcessStart += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameProcessStart(args);
        }
        [Fact]
        public void GameQuitEvent_Test()
        {
            var args = new GameQuitEventArgs(new FakeCoreService(), new FakeGameInfo(), new FakeEmulatorAssembly(), new FakeEmulatorBridge());
            SnowflakeEventSource.EventSource.GameQuit += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameQuit(args);
        }
        [Fact]
        public void GameStartEvent_Test()
        {
            var args = new GameStartEventArgs(new FakeCoreService(), new FakeGameInfo(), new FakeEmulatorAssembly(), new FakeEmulatorBridge());
            SnowflakeEventSource.EventSource.GameStart += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameStart(args);
        }
        [Fact]
        public void GameInfoScrapedEvent_Test()
        {
            var args = new GameInfoScrapedEventArgs(new FakeCoreService(), new FakeGameInfo(), new FakeScraper());
            SnowflakeEventSource.EventSource.GameInfoScraped += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameInfoScraped(args);
        }
        [Fact]
        public void GameScrapeResultsEvent_Test()
        {
            var args = new GameResultsScrapedEventArgs(new FakeCoreService(), "TEST", new List<Snowflake.Scraper.IGameScrapeResult>(), new FakeScraper());
            SnowflakeEventSource.EventSource.GameResultsScraped += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameResultScraped(args);
        }
        [Fact]
        public void ModifyControllerProfileEvent_Test()
        {
            var args = new ModifyControllerProfileEventArgs(new FakeCoreService(), new FakeControllerProfile(), new FakeControllerProfile());
            SnowflakeEventSource.EventSource.ControllerProfileModify += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnControllerProfileModify(args);
        }
        [Fact]
        public void ModifyConfigurationFlagEvent_Test()
        {
            var args = new ModifyConfigurationFlagEventArgs(new FakeCoreService(), new object(), new object(), new FakeConfigurationFlag());
            SnowflakeEventSource.EventSource.ConfigurationFlagModify += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnConfigurationFlagModify(args);
        }
        [Fact]
        public void ModifyGameInfoEvent_Test()
        {
            var args = new ModifyGameInfoEventArgs(new FakeCoreService(), new FakeGameInfo(), new FakeGameInfo());
            SnowflakeEventSource.EventSource.GameInfoModify += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameInfoModify(args);
        }
        [Fact]
        public void ModifyPlatformPreferenceEvent_Test()
        {
            var args = new ModifyPlatformPreferenceEventArgs(new FakeCoreService(), "test", "test", new FakePlatformInfo(), PreferenceType.PREF_EMULATOR);
            SnowflakeEventSource.EventSource.PlatformPreferenceModify += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnPlatformPreferenceModify(args);
        }
        [Fact]
        public void ModifyPortInputDeviceEvent_Test()
        {
            var args = new ModifyPortInputDeviceEventArgs(new FakeCoreService(), 1, new FakePlatformInfo(), Snowflake.Controller.InputDeviceNames.KeyboardDevice, Snowflake.Controller.InputDeviceNames.KeyboardDevice);
            SnowflakeEventSource.EventSource.PortInputDeviceModify += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnPortInputDeviceModify(args);
        }
        [Fact]
        public void CoreLoadedEvent_Test()
        {
            var args = new CoreLoadedEventArgs(new FakeCoreService());
            SnowflakeEventSource.EventSource.CoreLoaded += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnCoreLoaded(args);
        }
        [Fact]
        public void CoreShutdownEvent_Test()
        {
            var args = new CoreShutdownEventArgs(new FakeCoreService());
            SnowflakeEventSource.EventSource.CoreShutdown += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnCoreShutdown(args);
        }
        [Fact]
        public void EmulatorPromptEvent_Test()
        {
            var args = new EmulatorPromptEventArgs(new FakeCoreService(), "testprompt", "testbridge");
            SnowflakeEventSource.EventSource.EmulatorPrompt += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnEmulatorPrompt(args);
        }
        [Fact]
        public void AjaxRequestReceivedEvent_Test()
        {
            var args = new AjaxRequestReceivedEventArgs(new FakeCoreService(), new FakeJSRequest());
            SnowflakeEventSource.EventSource.AjaxRequestReceived += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnAjaxRequestReceived(args);
        }
        [Fact]
        public void AjaxResponseSendingEvent_Test()
        {
            var args = new AjaxResponseSendingEventArgs(new FakeCoreService(), new FakeJSResponse());
            SnowflakeEventSource.EventSource.AjaxResponseSending += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnAjaxResponseSending(args);
        }

    }

}
