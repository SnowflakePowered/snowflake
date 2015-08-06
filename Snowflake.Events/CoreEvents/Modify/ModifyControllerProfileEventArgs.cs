using Snowflake.Controller;
using Snowflake.Service;

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
