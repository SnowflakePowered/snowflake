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
    public class GameStartEventArgs : GameEventArgs
    {
        public IEmulatorAssembly GameEmulatorAssembly { get; set; }
        public IEmulatorBridge GameEmulatorBridge { get; set; }
        public GameStartEventArgs(ICoreService eventCoreInstance, IGameInfo gameInfo, IEmulatorAssembly emulatorAssembly, IEmulatorBridge emulatorBridge)
            : base(eventCoreInstance, gameInfo)
        {
            this.GameEmulatorAssembly = emulatorAssembly;
            this.GameEmulatorBridge = this.GameEmulatorBridge;
        }

    
    }
}
