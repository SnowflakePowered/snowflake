using Snowflake.Service;

namespace Snowflake.Events.ServiceEvents
{
    public class UIStopEventArgs : SnowflakeEventArgs
    {
        public IUserInterface UserInterface { get; }
        public UIStopEventArgs(ICoreService eventCoreInstance, IUserInterface userInterface)
            : base(eventCoreInstance)
        {
            this.UserInterface = userInterface;
        }
    }
}
