using Snowflake.Configuration.Input;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Orchestration.Saving;
using Snowflake.Extensibility;
using Snowflake.Input.Device;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Orchestration.Process;

namespace Snowflake.Plugin.Emulators.bsnes
{
    [Plugin("bsnes-Executor")]
    public class BsnesExecutor : EmulatorExecutor
    {
        public BsnesExecutor(IEmulatorExecutable bsnesExecutable)
            : base(typeof(BsnesExecutor))
        {
            this.BsnesExecutable = bsnesExecutable;
        }

        private IEmulatorExecutable BsnesExecutable { get; }

        protected override GameEmulation 
            ProvisionEmulationInstance(IGame game, IList<IEmulatedController> controllerPorts, 
            string configurationProfileName)
        {
            var gameEmulation = new HiganGameEmulation(game,
                new Dictionary<InputDriverType, IDeviceInputMapping>(),
                this.BsnesExecutable);
            return gameEmulation;
        }
    }
}
