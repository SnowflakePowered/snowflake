using Snowflake.Records.Game;
using Snowflake.Service;

namespace Snowflake.Events.CoreEvents.GameEvent
{
    public class GameAddEventArgs : GameEventArgs
    {
        public IGameLibrary GameLibrary { get; private set; }
        public GameAddEventArgs(ICoreService eventCoreInstance, IGameRecord gameInfo, IGameLibrary gameLibrary)
            : base(eventCoreInstance, gameInfo)
        {
            this.GameLibrary = gameLibrary;
        }

    
    }
}
