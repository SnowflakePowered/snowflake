using System.Diagnostics;
using Snowflake.Emulator;
using Snowflake.Game;
using Snowflake.Service;

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
            this.GameEmulatorBridge = this.GameEmulatorBridge;
            this.GameEmulatorProcess = this.GameEmulatorProcess;
        }

    
    }
}
