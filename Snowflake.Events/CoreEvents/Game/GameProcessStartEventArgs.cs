using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Snowflake.Service;
using Snowflake.Emulator;
using Snowflake.Game;

namespace Snowflake.Events.CoreEvents.GameEvent
{
    public class GameProcessStartEventArgs : GameEventArgs
    {
        public IEmulatorAssembly GameEmulatorAssembly { get; private set; }
        public IEmulatorBridge GameEmulatorBridge { get; }
        public Process GameEmulatorProcess { get; }
        public GameProcessStartEventArgs(ICoreService eventCoreInstance, IGameInfo gameInfo, IEmulatorAssembly emulatorAssembly, IEmulatorBridge emulatorBridge, Process process)
            : base(eventCoreInstance, gameInfo)
        {
            this.GameEmulatorAssembly = emulatorAssembly;
            this.GameEmulatorBridge = GameEmulatorBridge;
            this.GameEmulatorProcess = GameEmulatorProcess;
        }

    
    }
}
