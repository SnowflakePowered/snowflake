using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Snowflake.Events
{
    public class SnowflakeEventSource
    {
        public delegate void 
        public event OriginCloseEvent OriginClose;


        public void RegisterEvent(Func<object, SnowflakeEventArgs> eventHandler, string eventName)
        {

        }
    }
}
