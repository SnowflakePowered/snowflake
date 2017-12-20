using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Execution.Extensibility;
using Snowflake.Execution.Process;
using Snowflake.Execution.Saving;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Higan.Configuration;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Higan.Selections;
using Snowflake.Plugin.Emulators.RetroArch.Input;
using Snowflake.Records.Game;
using Snowflake.Services;

namespace Snowflake.Adapters.Higan
{
    [Plugin("RetroArchBsnes")]
    public sealed class HiganSnesAdapter : ExternalEmulator
    {
        public HiganSnesAdapter(IPluginProvision provision,
            IStoneProvider stone,
            IEmulatorTaskRootDirectoryProvider provider,
            IEmulatorExecutableProvider emulatorProvider,
            IConfigurationCollectionStore store)
            : base(provision, stone)
        {
            this.Runner = new HiganTaskRunner(emulatorProvider.GetEmulator("RetroArch"),
                provision);
            this.ConfigurationFactory = new HiganConfigurationFactory(provision, store);
            this.TaskRootProvider = provider;
        }

        public override IEmulatorTaskRunner Runner { get; }

        private ConfigurationFactory<HiganRetroArchConfiguration, RetroPadTemplate> ConfigurationFactory { get; }

        private IEmulatorTaskRootDirectoryProvider TaskRootProvider { get; }

        public override IEmulatorTask CreateTask(IGameRecord executingGame,
            ISaveLocation saveLocation,
            IList<IEmulatedController> controllerConfiguration,
            string profileContext = "default")
        {
            IConfigurationCollection<HiganRetroArchConfiguration> configuration =
            this.ConfigurationFactory.GetConfiguration(executingGame, profileContext);

            var templates = controllerConfiguration.Select(c => this.ConfigurationFactory.GetInputMappings(c)).ToList();
            var task = new EmulatorTask(executingGame)
            {
                GameSaveLocation = saveLocation,
                ControllerConfiguration = templates
                // todo: refactor out into extension method.
                    .Select(t => (t.template as IInputTemplate, t.mapping)).ToList(),
            };
            task.ProcessTaskRoot = new RetroArchTaskRoot(this.TaskRootProvider.GetTaskRoot(task));

            configuration.Configuration.DirectoryConfiguration.SavefileDirectory =
                task.ProcessTaskRoot.SaveDirectory.FullName;
            configuration.Configuration.DirectoryConfiguration.SystemDirectory =
                task.ProcessTaskRoot.SystemDirectory.FullName;
            configuration.Configuration.DirectoryConfiguration.CoreOptionsPath = 
                Path.Combine(task.ProcessTaskRoot.ConfigurationDirectory.FullName,
                "retroarch-core-options.cfg");

            switch (configuration.Configuration.BsnesCoreConfig.PerformanceProfile)
            {
                case PerformanceProfile.Performance:
                    task.AddPragma("retroarch_core", "bsnes_performance_libretro.dll");
                    break;
                case PerformanceProfile.Accuracy:
                    task.AddPragma("retroarch_core", "bsnes_accuracy_libretro.dll");
                    break;
                case PerformanceProfile.Balanced:
                    task.AddPragma("retroarch_core", "bsnes_balanced_libretro.dll");
                    break;
            }

            task.TaskConfiguration = configuration;
            return task;
        }
    }
}
