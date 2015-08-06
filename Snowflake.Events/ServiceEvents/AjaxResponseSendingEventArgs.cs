using Snowflake.Ajax;
using Snowflake.Service;

namespace Snowflake.Events.ServiceEvents
{
    public class AjaxRequestReceivedEventArgs : SnowflakeEventArgs
    {
        public IJSRequest ReceivedRequest { get; set; }
        public AjaxRequestReceivedEventArgs(ICoreService eventCoreInstance, IJSRequest receivedRequest) : base (eventCoreInstance)
        {
            this.ReceivedRequest = receivedRequest;
        }
    }
}
