using Snowflake.Emulator;
using Snowflake.Game;
using Snowflake.Service;

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
