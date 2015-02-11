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
        public void GameIdentifiedEvent_Test()
        {
            var args = new GameIdentifiedEventArgs(new FakeCoreService(), "test", "test.rom", new FakePlatformInfo());
            SnowflakeEventSource.EventSource.GameIdentified += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameIdentified(args);
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
        public void GameScrapedEvent_Test()
        {
            var args = new GameScrapedEventArgs(new FakeCoreService(), new FakeGameInfo(), new FakeScraper());
            SnowflakeEventSource.EventSource.GameScraped += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameScraped(args);
        }
       
    }

}
