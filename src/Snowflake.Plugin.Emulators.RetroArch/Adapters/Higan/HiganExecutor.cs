﻿using Snowflake.Configuration.Input;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Orchestration.Saving;
using Snowflake.Extensibility;
using Snowflake.Input.Device;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Adapters.Higan
{
    [Plugin("RetroArch-HiganExecutor")]
    public class HiganExecutor : EmulatorExecutor
    {
        public HiganExecutor() 
            : base(typeof(HiganExecutor))
        {
        }

        protected override GameEmulation 
            ProvisionEmulationInstance(IGame game, IList<IEmulatedController> controllerPorts, 
            string configurationProfileName)
        {
            var gameEmulation = new HiganGameEmulation(game,
                new Dictionary<InputDriverType, IDeviceInputMapping>(), null);
            return gameEmulation;
        }
    }
}
