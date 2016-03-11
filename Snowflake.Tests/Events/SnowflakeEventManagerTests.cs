using System;
using System.Threading;
using Moq;
using Snowflake.Service;
using Xunit;

namespace Snowflake.Events.Test
{
    public class SnowflakeEventManagerTests
    {
       
        [Fact]
        public void RegisterEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            SnowflakeEventManager.EventSource.RegisterEvent<SnowflakeEventArgs>(null);
            Assert.True(SnowflakeEventManager.EventSource.Contains<SnowflakeEventArgs>());
        }
        [Fact]
        public void UnregisterEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            SnowflakeEventManager.EventSource.RegisterEvent<SnowflakeEventArgs>(null);
            SnowflakeEventManager.EventSource.UnregisterEvent<SnowflakeEventArgs>();
            Assert.False(SnowflakeEventManager.EventSource.Contains<SnowflakeEventArgs>());
        }

        [Fact]
        public void SubscribeEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            SnowflakeEventManager.EventSource.RegisterEvent<SnowflakeEventArgs>(null);
            SnowflakeEventManager.EventSource.Subscribe<SnowflakeEventArgs>((s, e) =>
            {
                Assert.NotNull(e);
            });
            var fakeCoreService = new Mock<ICoreService>();
            SnowflakeEventManager.EventSource.RaiseEvent(new SnowflakeEventArgs(fakeCoreService.Object));
        }
        public void handleEvent(object sender, SnowflakeEventArgs args)
        {
            Assert.NotNull(args);
        }
        [Fact]
        public void UnsubscribeEvent_Test()
        {
            SnowflakeEventManager.InitEventSource();
            SnowflakeEventManager.EventSource.RegisterEvent<SnowflakeEventArgs>(null);
            SnowflakeEventManager.EventSource.Subscribe<SnowflakeEventArgs>(this.handleEvent);
            Assert.NotNull(SnowflakeEventManager.EventSource.GetEvent<SnowflakeEventArgs>());
            SnowflakeEventManager.EventSource.Unsubscribe<SnowflakeEventArgs>(this.handleEvent);
            Thread.Sleep(1000);
            Assert.Null(SnowflakeEventManager.EventSource.GetEvent<SnowflakeEventArgs>());
        }
    }
}
