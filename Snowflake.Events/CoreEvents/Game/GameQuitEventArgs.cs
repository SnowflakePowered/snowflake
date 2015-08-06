using Snowflake.Emulator;
using Snowflake.Game;
using Snowflake.Service;

namespace Snowflake.Events.CoreEvents.GameEvent
{
    public class GameQuitEventArgs : GameEventArgs
    {
        public IEmulatorAssembly GameEmulatorAssembly { get; private set; }
        public IEmulatorBridge GameEmulatorBridge { get; }
        public GameQuitEventArgs(ICoreService eventCoreInstance, IGameInfo gameInfo, IEmulatorAssembly emulatorAssembly, IEmulatorBridge emulatorBridge)
            : base(eventCoreInstance, gameInfo)
        {
            this.GameEmulatorAssembly = emulatorAssembly;
            this.GameEmulatorBridge = this.GameEmulatorBridge;
        }

    
    }
}
