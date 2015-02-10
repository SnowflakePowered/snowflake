using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Events
{
    public partial class SnowflakeEventSource
    {
        public static SnowflakeEventSource SnowflakeEventSource;
        SnowflakeEventSource()
        {
        }
        public static void InitEventSource()
        {
            if (SnowflakeEventSource.SnowflakeEventSource == null)
            {
                SnowflakeEventSource.SnowflakeEventSource = new SnowflakeEventSource();
            }
        }
    }
}
