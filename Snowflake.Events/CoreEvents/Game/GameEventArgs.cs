using Snowflake.Game;
using Snowflake.Service;

namespace Snowflake.Events.CoreEvents.GameEvent
{
    public abstract class GameEventArgs : SnowflakeEventArgs
    {
        public IGameInfo GameInfo { get; set; }
        
        public GameEventArgs(ICoreService eventCoreInstance, IGameInfo gameInfo) : base(eventCoreInstance)
        {
            this.GameInfo = gameInfo;
        }
    }
}
