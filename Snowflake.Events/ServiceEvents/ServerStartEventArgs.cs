using Snowflake.Service;

namespace Snowflake.Events.ServiceEvents
{
    public class ServerStartEventArgs: SnowflakeEventArgs
    {
        public string ServerName { get; private set; }
        public ServerStartEventArgs(ICoreService eventCoreInstance, string serverName)
            : base(eventCoreInstance)
        {
            this.ServerName = serverName;
        }
    }
}
