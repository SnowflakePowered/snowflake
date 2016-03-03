using Snowflake.Game;
using Snowflake.Service;

namespace Snowflake.Events.CoreEvents.GameEvent
{
    public class GamePreAddEventArgs : GameEventArgs
    {
        public IGameLibrary GameLibrary { get; private set; }
        public GamePreAddEventArgs(ICoreService eventCoreInstance, IGameInfo gameInfo, IGameLibrary gameLibrary)
            : base(eventCoreInstance, gameInfo)
        {
            this.GameLibrary = gameLibrary;
        }

    
    }
}
