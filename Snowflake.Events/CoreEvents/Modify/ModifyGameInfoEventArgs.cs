using Snowflake.Game;
using Snowflake.Service;

namespace Snowflake.Events.CoreEvents.ModifyEvent
{
    public class ModifyGameInfoEventArgs : SnowflakeEventArgs
    {
        public IGameInfo PreviousGameInfo { get; private set; }
        public IGameInfo ModifiedGameInfo { get; set; }
        public ModifyGameInfoEventArgs(ICoreService eventCoreInstance, IGameInfo previousGameInfo, IGameInfo modifiedGameInfo)
            : base(eventCoreInstance)
        {
            this.PreviousGameInfo = previousGameInfo;
            this.ModifiedGameInfo = modifiedGameInfo;
        }
    }
}
