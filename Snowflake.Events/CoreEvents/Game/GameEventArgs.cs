using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Snowflake.Game;
using Snowflake.Emulator;
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
