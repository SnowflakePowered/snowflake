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
           var args = new GameAddEventArgs(new FakeCoreServices(), new FakeGameInfo(), new FakeGameDatabase());
           SnowflakeEventSource.EventSource.GameAdd += (s, e) =>
           {
               Assert.Equal(args, e);
           };
           SnowflakeEventSource.EventSource.OnGameAdd(args);
        }
        [Fact]
        public void GameDeleteEvent_Test()
        {
            var args = new GameDeleteEventArgs(new FakeCoreServices(), new FakeGameInfo(), new FakeGameDatabase());
            SnowflakeEventSource.EventSource.GameDelete += (s, e) =>
            {
                Assert.Equal(args, e);
            };
            SnowflakeEventSource.EventSource.OnGameDelete(args);
        }
    }

}
