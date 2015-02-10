using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Snowflake.Emulator;
using Snowflake.Game;

namespace Snowflake.Events.CoreEvents
{
    public class GameQuitEventArgs : GameEventArgs
    {
        public IEmulatorAssembly GameEmulatorAssembly { get; private set; }
        public IEmulatorBridge GameEmulatorBridge { get; private set; }
        public GameQuitEventArgs(ICoreService eventCoreInstance, IGameInfo gameInfo, IEmulatorAssembly emulatorAssembly, IEmulatorBridge emulatorBridge)
            : base(eventCoreInstance, gameInfo)
        {
            this.GameEmulatorAssembly = emulatorAssembly;
            this.GameEmulatorBridge = GameEmulatorBridge;
        }

    
    }
}
