﻿
using Snowflake.Records.Game;
using Snowflake.Service;

namespace Snowflake.Events.CoreEvents.GameEvent
{
    public class GamePreDeleteEventArgs : GameEventArgs
    {
        public IGameLibrary GameLibrary { get; private set; }
        public GamePreDeleteEventArgs(ICoreService eventCoreInstance, IGameRecord gameInfo, IGameLibrary gameLibrary)
            : base(eventCoreInstance, gameInfo)
        {
            this.GameLibrary = gameLibrary;
        }

    
    }
}
