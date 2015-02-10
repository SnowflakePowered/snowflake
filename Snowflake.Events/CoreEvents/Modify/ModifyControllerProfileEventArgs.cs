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
        public IControllerProfile PreviousProfile { get; private set; }
        public IControllerProfile ModifiedProfile { get; set; }
        public ModifyControllerProfileEventArgs(ICoreService eventCoreInstance, IControllerProfile previousProfile, IControllerProfile modifiedProfile)
            : base(eventCoreInstance)
        {
            this.PreviousProfile = previousProfile;
            this.ModifiedProfile = modifiedProfile;
        }
    }
}
