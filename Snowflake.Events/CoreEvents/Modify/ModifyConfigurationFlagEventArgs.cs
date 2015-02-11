using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Configuration;
using Snowflake.Service;

namespace Snowflake.Events.CoreEvents.ModifyEvent
{
    public class ModifyConfigurationFlagEventArgs : SnowflakeEventArgs
    {
        public IConfigurationFlag ConfigurationFlag { get; private set; }
        public object PreviousFlagValue { get; private set; }
        public object ModifiedFlagValue { get; set; }

        public ModifyConfigurationFlagEventArgs(ICoreService eventCoreInstance, object previousFlagValue, object modifiedFlagValue, IConfigurationFlag configurationFlag): base(eventCoreInstance)
        {
            this.PreviousFlagValue = previousFlagValue;
            this.ModifiedFlagValue = modifiedFlagValue;
            this.ConfigurationFlag = configurationFlag;
        }
    }
}
