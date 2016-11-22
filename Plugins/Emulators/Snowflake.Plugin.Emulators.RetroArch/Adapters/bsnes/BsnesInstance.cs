﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator;
using Snowflake.Platform;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.bsnes.Configuration;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.bsnes.Selections;
using Snowflake.Plugin.Emulators.RetroArch.Executable;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Configuration;

namespace Snowflake.Plugin.Emulators.RetroArch.Adapters.bsnes
{
    internal sealed class BsnesInstance : RetroArchInstance
    {
        public BsnesInstance(IGameRecord game, IFileRecord file, RetroArchCommonAdapter adapter, string corePath, RetroArchProcessHandler processHandler, int saveSlot, IPlatformInfo platform, IList<IEmulatedPort> controllerPorts) 
            : base(game, file, adapter, corePath, processHandler, saveSlot, platform, controllerPorts)
        {
            switch (((this.Configuration) as IConfigurationCollection<BsnesConfiguration>).Configuration.BsnesCoreConfig.PerformanceProfile)
            {
                case PerformanceProfile.Performance:
                    this.CorePath = Path.Combine(adapter.PluginDataPath, "bsnes_performance_libretro.dll");
                    break;
                case PerformanceProfile.Accuracy:
                    this.CorePath = Path.Combine(adapter.PluginDataPath, "bsnes_accuracy_libretro.dll");
                    break;
                case PerformanceProfile.Balanced:
                    this.CorePath = Path.Combine(adapter.PluginDataPath, "bsnes_balanced_libretro.dll");
                    break;
            }
        }
    }
}
