
using Snowflake.Records.Game;
using Snowflake.Services;

namespace Snowflake.Events.CoreEvents.ModifyEvent
{
    public class ModifyGameInfoEventArgs : SnowflakeEventArgs
    {
        public IGameRecord PreviousGameInfo { get; private set; }
        public IGameRecord ModifiedGameInfo { get; set; }
        public ModifyGameInfoEventArgs(ICoreService eventCoreInstance, IGameRecord previousGameInfo, IGameRecord modifiedGameInfo)
            : base(eventCoreInstance)
        {
            this.PreviousGameInfo = previousGameInfo;
            this.ModifiedGameInfo = modifiedGameInfo;
        }
    }
}
