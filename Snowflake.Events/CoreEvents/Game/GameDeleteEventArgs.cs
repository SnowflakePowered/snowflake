using Snowflake.Game;
using Snowflake.Service;

namespace Snowflake.Events.CoreEvents.GameEvent
{
    public class GameDeleteEventArgs : GameEventArgs
    {
        public IGameDatabase GameDatabase { get; private set; }
        public GameDeleteEventArgs(ICoreService eventCoreInstance, IGameInfo gameInfo, IGameDatabase gameDatabase)
            : base(eventCoreInstance, gameInfo)
        {
            this.GameDatabase = gameDatabase;
        }

    
    }
}
