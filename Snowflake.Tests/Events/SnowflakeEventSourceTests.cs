using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Events;
using Snowflake.Events.CoreEvents.GameEvent;
using Snowflake.Events.CoreEvents.ModifyEvent;
using Snowflake.Events.ServiceEvents;
using Snowflake.Service;
using Snowflake.Game;
using Snowflake.Platform;
using Snowflake.Emulator;
using Snowflake.Scraper;
using Snowflake.Emulator.Configuration;
using Snowflake.Emulator.Input;
using Snowflake.Ajax;
using Xunit;
using Moq;
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
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var fakeGameDatabase = new Mock<IGameDatabase>();
            var args = new GameAddEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeGameDatabase.Object);
            SnowflakeEventSource.EventSource.GameAdd += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameAdd(args);
        }
        [Fact]
        public void GameDeleteEvent_Test()
        {
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var fakeGameDatabase = new Mock<IGameDatabase>();
            var args = new GameDeleteEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeGameDatabase.Object);
            SnowflakeEventSource.EventSource.GameDelete += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameDelete(args);
        }
        [Fact]
        public void GameProcessQuitEventArgs_Test()
        {
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var fakeEmulatorAssembly = new Mock<IEmulatorAssembly>();
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            var args = new GameProcessQuitEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeEmulatorAssembly.Object, fakeEmulatorBridge.Object, new System.Diagnostics.Process());
            SnowflakeEventSource.EventSource.GameProcessQuit += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameProcessQuit(args);
        }
        [Fact]
        public void GameProcessStartEventArgs_Test()
        {
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var fakeEmulatorAssembly = new Mock<IEmulatorAssembly>();
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            var args = new GameProcessStartEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeEmulatorAssembly.Object, fakeEmulatorBridge.Object, new System.Diagnostics.Process());
            SnowflakeEventSource.EventSource.GameProcessStart += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameProcessStart(args);
        }
        [Fact]
        public void GameQuitEvent_Test()
        {
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var fakeEmulatorAssembly = new Mock<IEmulatorAssembly>();
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            var args = new GameQuitEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeEmulatorAssembly.Object, fakeEmulatorBridge.Object);
            SnowflakeEventSource.EventSource.GameQuit += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameQuit(args);
        }
        [Fact]
        public void GameStartEvent_Test()
        {
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var fakeEmulatorAssembly = new Mock<IEmulatorAssembly>();
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            var args = new GameStartEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeEmulatorAssembly.Object, fakeEmulatorBridge.Object);
            SnowflakeEventSource.EventSource.GameStart += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameStart(args);
        }
        [Fact]
        public void GameInfoScrapedEvent_Test()
        {
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var fakeScraper = new Mock<IScraper>();
            var args = new GameInfoScrapedEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeScraper.Object);
            SnowflakeEventSource.EventSource.GameInfoScraped += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameInfoScraped(args);
        }
        [Fact]
        public void GameScrapeResultsEvent_Test()
        {
            var fakeCoreService = new Mock<ICoreService>();
            var fakeScraper = new Mock<IScraper>();
            var args = new GameResultsScrapedEventArgs(fakeCoreService.Object, "TEST", new List<Snowflake.Scraper.IGameScrapeResult>(), fakeScraper.Object);
            SnowflakeEventSource.EventSource.GameResultsScraped += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameResultScraped(args);
        }
      
        [Fact]
        public void ModifyConfigurationFlagEvent_Test()
        {
            var fakeCoreService = new Mock<ICoreService>();
            var fakeConfiguration = new Mock<IConfigurationFlag>();
            var args = new ModifyConfigurationFlagEventArgs(fakeCoreService.Object, new object(), new object(), fakeConfiguration.Object);
            SnowflakeEventSource.EventSource.ConfigurationFlagModify += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnConfigurationFlagModify(args);
        }
        [Fact]
        public void ModifyGameInfoEvent_Test()
        {
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var args = new ModifyGameInfoEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeGameInfo.Object);
            SnowflakeEventSource.EventSource.GameInfoModify += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameInfoModify(args);
        }
        [Fact]
        public void ModifyPlatformPreferenceEvent_Test()
        {
            var fakeCoreService = new Mock<ICoreService>();
            var fakePlatformInfo = new Mock<IPlatformInfo>();
            var args = new ModifyPlatformPreferenceEventArgs(fakeCoreService.Object, "test", "test", fakePlatformInfo.Object, PreferenceType.PREF_EMULATOR);
            SnowflakeEventSource.EventSource.PlatformPreferenceModify += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnPlatformPreferenceModify(args);
        }
        [Fact]
        public void ModifyPortInputDeviceEvent_Test()
        {
            var fakeCoreService = new Mock<ICoreService>();
            var fakePlatformInfo = new Mock<IPlatformInfo>();
            var args = new ModifyPortInputDeviceEventArgs(fakeCoreService.Object, 1, fakePlatformInfo.Object, Snowflake.Controller.InputDeviceNames.KeyboardDevice, Snowflake.Controller.InputDeviceNames.KeyboardDevice);
            SnowflakeEventSource.EventSource.PortInputDeviceModify += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnPortInputDeviceModify(args);
        }
        [Fact]
        public void CoreLoadedEvent_Test()
        {
            var fakeCoreService = new Mock<ICoreService>();
            var args = new CoreLoadedEventArgs(fakeCoreService.Object);
            SnowflakeEventSource.EventSource.CoreLoaded += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnCoreLoaded(args);
        }
        [Fact]
        public void CoreShutdownEvent_Test()
        {
            var fakeCoreService = new Mock<ICoreService>();
            var args = new CoreShutdownEventArgs(fakeCoreService.Object);
            SnowflakeEventSource.EventSource.CoreShutdown += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnCoreShutdown(args);
        }
        [Fact]
        public void AjaxRequestReceivedEvent_Test()
        {
            var fakeCoreService = new Mock<ICoreService>();
            var fakeJsRequest = new Mock<IJSRequest>();
            var args = new AjaxRequestReceivedEventArgs(fakeCoreService.Object, fakeJsRequest.Object);
            SnowflakeEventSource.EventSource.AjaxRequestReceived += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnAjaxRequestReceived(args);
        }
        [Fact]
        public void AjaxResponseSendingEvent_Test()
        {
            var fakeCoreService = new Mock<ICoreService>();
            var fakeJsResponse = new Mock<IJSResponse>();
            var args = new AjaxResponseSendingEventArgs(fakeCoreService.Object, fakeJsResponse.Object);
            SnowflakeEventSource.EventSource.AjaxResponseSending += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnAjaxResponseSending(args);
        }

    }

}
