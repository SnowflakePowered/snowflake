using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;

namespace Snowflake.Events.ServiceEvents
{
    public class CoreLoadedEventArgs : SnowflakeEventArgs
    {
        public CoreLoadedEventArgs(ICoreService eventCoreInstance) : base(eventCoreInstance)
        {
            
        }
    }
}
