using Snowflake.Service;

namespace Snowflake.Events
{
    public class SnowflakeEventArgs 
    {
        public ICoreService EventCoreInstance { get; private set;}
        public bool Cancel { get; set; }
    
        public SnowflakeEventArgs(ICoreService eventCoreInstance){
            this.EventCoreInstance = eventCoreInstance;
            this.Cancel = false;
        }
    }
}
