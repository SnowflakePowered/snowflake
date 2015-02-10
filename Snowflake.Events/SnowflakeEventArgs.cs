using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;

namespace Snowflake.Events
{
    internal class SnowflakeEventArgs
    {
        public ICoreService EventCoreService { get; }
    }
}
