using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Events.ServiceEvents;
namespace Snowflake.Events
{
    public partial class SnowflakeEventSource
    {
        public event EventHandler<AjaxRequestReceivedEventArgs> AjaxRequestReceived;
        public event EventHandler<AjaxResponseSendingEventArgs> AjaxResponseSending;
        public event EventHandler<CoreLoadedEventArgs> CoreLoaded;
        public event EventHandler<CoreShutdownEventArgs> CoreShutdown;
        public event EventHandler<ServerStartEventArgs> ServerStart;
        public event EventHandler<ServerStopEventArgs> ServerStop;

        public void OnAjaxRequestReceived(AjaxRequestReceivedEventArgs e)
        {
            if (this.AjaxRequestReceived != null)
            {
                this.AjaxRequestReceived(this, e);
            }
        }
        public void OnAjaxResponseSending(AjaxResponseSendingEventArgs e)
        {
            if (this.AjaxResponseSending != null)
            {
                this.AjaxResponseSending(this, e);
            }
        }
        public void OnCoreLoaded(CoreLoadedEventArgs e)
        {
            if (this.CoreLoaded != null)
            {
                this.CoreLoaded(this, e);
            }
        }
        public void OnCoreShutdown(CoreShutdownEventArgs e)
        {
            if (this.CoreShutdown != null)
            {
                this.CoreShutdown(this, e);
            }
        }
        public void OnServerStart(ServerStartEventArgs e)
        {
            if (this.ServerStart != null)
            {
                this.ServerStart(this, e);
            }
        }
        public void OnServerStop(ServerStopEventArgs e)
        {
            if (this.ServerStop != null)
            {
                this.ServerStop(this, e);
            }
        }
    }
}
