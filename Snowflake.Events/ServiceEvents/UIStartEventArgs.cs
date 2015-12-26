using Snowflake.Service;

namespace Snowflake.Events.ServiceEvents
{
    public class UIStartEventArgs : SnowflakeEventArgs
    {
        public IUserInterface UserInterface { get; }
        public UIStartEventArgs(ICoreService eventCoreInstance, IUserInterface userInterface)
            : base(eventCoreInstance)
        {
            this.UserInterface = userInterface;
        }
    }
}
