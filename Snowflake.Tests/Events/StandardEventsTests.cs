using System.Collections.Generic;
using Moq;
using Snowflake.Ajax;
using Snowflake.Emulator;
using Snowflake.Events.CoreEvents.GameEvent;
using Snowflake.Events.CoreEvents.ModifyEvent;
using Snowflake.Events.ServiceEvents;
using Snowflake.Platform;
using Snowflake.Scraper;
using Snowflake.Service;
using Xunit;

namespace Snowflake.Events.Tests
{

    public class SnowflakeEventSourceTests
    {

       
      /*  [Fact]
        public void GameProcessQuitEventArgs_Test()
        {
            SnowflakeEventManager.InitEventSource();
            new StandardEvents().RegisterSnowflakeEvents(SnowflakeEventManager.EventSource);
            var fakeCoreService = new Mock<ICoreService>();
            var fakeGameInfo = new Mock<IGameInfo>();
            var fakeEmulatorAssembly = new Mock<IEmulatorAssembly>();
            var fakeEmulatorBridge = new Mock<IEmulatorBridge>();
            var args = new GameProcessQuitEventArgs(fakeCoreService.Object, fakeGameInfo.Object, fakeEmulatorAssembly.Object, fakeEmulatorBridge.Object, new System.Diagnostics.Process());
            SnowflakeEventManager.EventSource.Subscribe<GameProcessQuitEventArgs>("TestHandler", (s, e) =>
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
            SnowflakeEventManager.EventSource.Subscribe<GameProcessStartEventArgs>("TestHandler", (s, e) =>
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
            SnowflakeEventManager.EventSource.Subscribe<GameQuitEventArgs>("TestHandler", (s, e) =>
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
            SnowflakeEventManager.EventSource.Subscribe<GameStartEventArgs>("TestHandler", (s, e) =>
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
            SnowflakeEventManager.EventSource.Subscribe<GameInfoScrapedEventArgs>("TestHandler", (s, e) =>
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
            var args = new GameResultsScrapedEventArgs(fakeCoreService.Object, "TEST", new List<IGameScrapeResult>(), fakeScraper.Object);
            SnowflakeEventManager.EventSource.Subscribe<GameResultsScrapedEventArgs>("TestHandler", (s, e) =>
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
            SnowflakeEventManager.EventSource.Subscribe<ModifyConfigurationFlagEventArgs>("TestHandler", (s, e) =>
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
            SnowflakeEventManager.EventSource.Subscribe<ModifyGameInfoEventArgs>("TestHandler", (s, e) =>
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
            SnowflakeEventManager.EventSource.Subscribe<ModifyPlatformPreferenceEventArgs>("TestHandler", (s, e) =>
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
            var args = new ModifyPortInputDeviceEventArgs(fakeCoreService.Object, 1, fakePlatformInfo.Object, Controller.InputDeviceNames.KeyboardDevice, Controller.InputDeviceNames.KeyboardDevice);
            SnowflakeEventManager.EventSource.Subscribe<ModifyPortInputDeviceEventArgs>("TestHandler", (s, e) =>
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
            SnowflakeEventManager.EventSource.Subscribe<CoreLoadedEventArgs>("TestHandler", (s, e) =>
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
            SnowflakeEventManager.EventSource.Subscribe<CoreShutdownEventArgs>("TestHandler", (s, e) =>
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
            SnowflakeEventManager.EventSource.Subscribe<AjaxRequestReceivedEventArgs>("TestHandler", (s, e) =>
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
            SnowflakeEventManager.EventSource.Subscribe<AjaxResponseSendingEventArgs>("TestHandler", (s, e) =>
            {
                Assert.Equal(args, e);
            });
            SnowflakeEventManager.EventSource.RaiseEvent(args);
        }*/

    }

}