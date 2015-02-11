using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;

namespace Snowflake.Events
{
    public class SnowflakeEventArgs 
    {
        public ICoreService EventCoreInstance { get; private set;}
        public bool Cancel { get; set; }
    
        internal SnowflakeEventArgs(ICoreService eventCoreInstance){
            this.EventCoreInstance = eventCoreInstance;
            this.Cancel = false;
        }
    }
}
