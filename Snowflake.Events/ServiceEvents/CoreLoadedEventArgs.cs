using Snowflake.Services;

namespace Snowflake.Events.ServiceEvents
{
    public class CoreLoadedEventArgs : SnowflakeEventArgs
    {
        public CoreLoadedEventArgs(ICoreService eventCoreInstance) : base(eventCoreInstance)
        {
            
        }
    }
}
