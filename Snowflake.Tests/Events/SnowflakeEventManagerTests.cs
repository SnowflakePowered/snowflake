using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Xunit;
using Moq;
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
            SnowflakeEventManager.EventSource.RaiseEvent<SnowflakeEventArgs>(new SnowflakeEventArgs(fakeCoreService.Object));
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
            Assert.Null(SnowflakeEventManager.EventSource.GetEvent<SnowflakeEventArgs>());
            var fakeCoreService = new Mock<ICoreService>();
            SnowflakeEventManager.EventSource.RaiseEvent<SnowflakeEventArgs>(new SnowflakeEventArgs(fakeCoreService.Object));
        }
    }
}
