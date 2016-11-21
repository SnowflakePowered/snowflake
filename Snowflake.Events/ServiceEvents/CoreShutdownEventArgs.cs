﻿using Snowflake.Services;

namespace Snowflake.Events.ServiceEvents
{
    public class CoreShutdownEventArgs : SnowflakeEventArgs
    {
        public CoreShutdownEventArgs(ICoreService eventCoreInstance)
            : base(eventCoreInstance)
        {

        }
    }
}
