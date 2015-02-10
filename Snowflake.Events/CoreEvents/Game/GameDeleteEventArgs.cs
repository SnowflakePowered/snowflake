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
