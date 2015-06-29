using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Snowflake.Controller;

namespace Snowflake.Events.CoreEvents.ModifyEvent
{
    public class ModifyControllerProfileEventArgs : SnowflakeEventArgs
    {
        public IGamepadAbstraction PreviousProfile { get; private set; }
        public IGamepadAbstraction ModifiedProfile { get; set; }
        public ModifyControllerProfileEventArgs(ICoreService eventCoreInstance, IGamepadAbstraction previousProfile, IGamepadAbstraction modifiedProfile)
            : base(eventCoreInstance)
        {
            this.PreviousProfile = previousProfile;
            this.ModifiedProfile = modifiedProfile;
        }
    }
}
