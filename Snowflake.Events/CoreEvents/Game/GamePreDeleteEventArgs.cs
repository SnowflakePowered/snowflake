using Snowflake.Game;
using Snowflake.Service;

namespace Snowflake.Events.CoreEvents.GameEvent
{
    public class GamePreDeleteEventArgs : GameEventArgs
    {
        public IGameDatabase GameDatabase { get; private set; }
        public GamePreDeleteEventArgs(ICoreService eventCoreInstance, IGameInfo gameInfo, IGameDatabase gameDatabase)
            : base(eventCoreInstance, gameInfo)
        {
            this.GameDatabase = gameDatabase;
        }

    
    }
}
