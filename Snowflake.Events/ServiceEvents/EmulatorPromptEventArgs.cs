using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
namespace Snowflake.Events.ServiceEvents
{
    public class EmulatorPromptEventArgs : SnowflakeEventArgs
    {
        public string PromptMessage { get; set; }
        public string TargetEmulatorBridgeName { get; private set; }

        public EmulatorPromptEventArgs(ICoreService eventCoreInstance, string promptMessage, string targetEmulatorBridgeName)
            : base(eventCoreInstance)
        {
            this.PromptMessage = promptMessage;
            this.TargetEmulatorBridgeName = targetEmulatorBridgeName;
        }
    }
}
