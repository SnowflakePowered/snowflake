using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Snowflake.Emulator;
using Snowflake.Game;

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
