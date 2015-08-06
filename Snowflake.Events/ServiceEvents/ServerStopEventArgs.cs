using Snowflake.Service;

namespace Snowflake.Events.ServiceEvents
{
    public class ServerStopEventArgs : SnowflakeEventArgs
    {
        public ServerStopEventArgs(ICoreService eventCoreInstance)
            : base(eventCoreInstance)
        {

        }
    }
}
