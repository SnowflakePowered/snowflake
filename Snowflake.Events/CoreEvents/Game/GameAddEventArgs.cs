using Snowflake.Game;
using Snowflake.Service;

namespace Snowflake.Events.CoreEvents.GameEvent
{
    public class GameAddEventArgs : GameEventArgs
    {
        public IGameDatabase GameDatabase { get; private set; }
        public GameAddEventArgs(ICoreService eventCoreInstance, IGameInfo gameInfo, IGameDatabase gameDatabase)
            : base(eventCoreInstance, gameInfo)
        {
            this.GameDatabase = gameDatabase;
        }

    
    }
}
