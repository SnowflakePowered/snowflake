using Snowflake.Ajax;
using Snowflake.Services;

namespace Snowflake.Events.ServiceEvents
{
    public class AjaxResponseSendingEventArgs : SnowflakeEventArgs
    {
        public IJSResponse SendingResponse { get; set; }
        public AjaxResponseSendingEventArgs(ICoreService eventCoreInstance, IJSResponse sendingResponse)
            : base(eventCoreInstance)
        {
            this.SendingResponse = sendingResponse;
        }
    }
}
