
using Snowflake.Records.Game;
using Snowflake.Services;

namespace Snowflake.Events.CoreEvents.GameEvent
{
    public abstract class GameEventArgs : SnowflakeEventArgs
    {
        public IGameRecord GameInfo { get; set; }
        
        protected GameEventArgs(ICoreService eventCoreInstance, IGameRecord gameInfo) : base(eventCoreInstance)
        {
            this.GameInfo = gameInfo;
        }
    }
}
