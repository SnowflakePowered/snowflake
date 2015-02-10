using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
namespace Snowflake.Events
{
    public class SnowflakeEventSource
    {

        public SnowflakeEventSource(ICoreService coreInstance){

        }
        public void RegisterEvent(Func<object, SnowflakeEventArgs> eventHandler, string eventName)
        {

        }
    }
}
