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

        [Fact]
        public void GameAddEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var fakeGameDatabase = new Mock<IGameDatabase>();
            var args = new GameAddEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeGameDatabase.Object);
            SnowflakeEventManager.EventSource.Subscribe<GameAddEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }
        [Fact]
        public void GameDeleteEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var fakeGameDatabase = new Mock<IGameDatabase>();
            var args = new GameDeleteEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeGameDatabase.Object);
            SnowflakeEventManager.EventSource.Subscribe<GameDeleteEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }
        [Fact]
        public void GameProcessQuitEventArgs_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var fakeEmulatorAssembly = new Mock<IEmulatorAssembly>();
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            var args = new GameProcessQuitEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeEmulatorAssembly.Object, fakeEmulatorBridge.Object, new System.Diagnostics.Process());
            SnowflakeEventManager.EventSource.Subscribe<GameProcessQuitEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }
        [Fact]
        public void GameProcessStartEventArgs_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var fakeEmulatorAssembly = new Mock<IEmulatorAssembly>();
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            var args = new GameProcessStartEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeEmulatorAssembly.Object, fakeEmulatorBridge.Object, new System.Diagnostics.Process());
            SnowflakeEventManager.EventSource.Subscribe<GameProcessStartEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }
        [Fact]
        public void GameQuitEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var fakeEmulatorAssembly = new Mock<IEmulatorAssembly>();
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            var args = new GameQuitEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeEmulatorAssembly.Object, fakeEmulatorBridge.Object);
            SnowflakeEventManager.EventSource.Subscribe<GameQuitEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }
        [Fact]
        public void GameStartEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var fakeEmulatorAssembly = new Mock<IEmulatorAssembly>();
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            var args = new GameStartEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeEmulatorAssembly.Object, fakeEmulatorBridge.Object);
            SnowflakeEventManager.EventSource.Subscribe<GameStartEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }
        [Fact]
        public void GameInfoScrapedEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var fakeScraper = new Mock<IScraper>();
            var args = new GameInfoScrapedEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeScraper.Object);
            SnowflakeEventManager.EventSource.Subscribe<GameInfoScrapedEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }
        [Fact]
        public void GameScrapeResultsEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var fakeScraper = new Mock<IScraper>();
            var args = new GameResultsScrapedEventArgs(fakeCoreService.Object, "TEST", new List<Snowflake.Scraper.IGameScrapeResult>(), fakeScraper.Object);
            SnowflakeEventManager.EventSource.Subscribe<GameResultsScrapedEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }

        [Fact]
        public void ModifyConfigurationFlagEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var fakeConfiguration = new Mock<IConfigurationFlag>();
            var args = new ModifyConfigurationFlagEventArgs(fakeCoreService.Object, new object(), new object(), fakeConfiguration.Object);
            SnowflakeEventManager.EventSource.Subscribe<ModifyConfigurationFlagEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }
        [Fact]
        public void ModifyGameInfoEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var args = new ModifyGameInfoEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeGameInfo.Object);
            SnowflakeEventManager.EventSource.Subscribe<ModifyGameInfoEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }
        [Fact]
        public void ModifyPlatformPreferenceEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var fakePlatformInfo = new Mock<IPlatformInfo>();
            var args = new ModifyPlatformPreferenceEventArgs(fakeCoreService.Object, "test", "test", fakePlatformInfo.Object, PreferenceType.PREF_EMULATOR);
            SnowflakeEventManager.EventSource.Subscribe<ModifyPlatformPreferenceEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }
        [Fact]
        public void ModifyPortInputDeviceEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var fakePlatformInfo = new Mock<IPlatformInfo>();
            var args = new ModifyPortInputDeviceEventArgs(fakeCoreService.Object, 1, fakePlatformInfo.Object, Snowflake.Controller.InputDeviceNames.KeyboardDevice, Snowflake.Controller.InputDeviceNames.KeyboardDevice);
            SnowflakeEventManager.EventSource.Subscribe<ModifyPortInputDeviceEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }
        [Fact]
        public void CoreLoadedEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var args = new CoreLoadedEventArgs(fakeCoreService.Object);
            SnowflakeEventManager.EventSource.Subscribe<CoreLoadedEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }
        [Fact]
        public void CoreShutdownEvent_Test()

        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var args = new CoreShutdownEventArgs(fakeCoreService.Object);
            SnowflakeEventManager.EventSource.Subscribe<CoreShutdownEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }
        [Fact]
        public void AjaxRequestReceivedEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var fakeJsRequest = new Mock<IJSRequest>();
            var args = new AjaxRequestReceivedEventArgs(fakeCoreService.Object, fakeJsRequest.Object);
            SnowflakeEventManager.EventSource.Subscribe<AjaxRequestReceivedEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }
        [Fact]
        public void AjaxResponseSendingEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var fakeJsResponse = new Mock<IJSResponse>();
            var args = new AjaxResponseSendingEventArgs(fakeCoreService.Object, fakeJsResponse.Object);
            SnowflakeEventManager.EventSource.Subscribe<AjaxResponseSendingEventArgs>((s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }

    }

}