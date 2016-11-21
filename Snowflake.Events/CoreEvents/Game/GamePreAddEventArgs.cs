
using Snowflake.Records.Game;
using Snowflake.Services;

namespace Snowflake.Events.CoreEvents.GameEvent
{
    public class GamePreAddEventArgs : GameEventArgs
    {
        public IGameLibrary GameLibrary { get; private set; }
        public GamePreAddEventArgs(ICoreService eventCoreInstance, IGameRecord gameInfo, IGameLibrary gameLibrary)
            : base(eventCoreInstance, gameInfo)
        {
            this.GameLibrary = gameLibrary;
        }

    
    }
}
