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
        public event EventHandler<EmulatorPromptEventArgs> EmulatorPrompt;

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
        public void OnEmulatorPrompt(EmulatorPromptEventArgs e)
        {
            if (this.EmulatorPrompt != null)
            {
                this.EmulatorPrompt(this, e);
            }
        }
    }
}
