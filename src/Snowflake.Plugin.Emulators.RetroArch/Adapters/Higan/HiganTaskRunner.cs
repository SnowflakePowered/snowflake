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
using Snowflake.Input.Device;

namespace Snowflake.Adapters.Higan
{
    public class HiganTaskRunner : IEmulatorTaskRunner
    {
        public IEmulatorExecutable RetroArchExecutable { get; }
        public IEmulatorTaskRootDirectoryProvider DirectoryProvider { get; }
        public async Task<IEmulatorTaskResult> ExecuteEmulationAsync(IEmulatorTask task)
        {
            IEmulatorTaskResult result = new RetroArchTaskResult(task.ProcessTaskRoot, task.GameSaveLocation);
          /*  IProcessBuilder builder = this.RetroArchExecutable.GetProcessBuilder();
            builder.WithArgument("--verbose")
                .WithArgument("-s", task.ProcessTaskRoot.SaveDirectory.FullName);*/
            foreach (var cfg in this.BuildConfiguration(task.TaskConfiguration, task.ControllerConfiguration))
            {
                await File.WriteAllTextAsync(Path.Combine(task.ProcessTaskRoot.ConfigurationDirectory.FullName,
                    task.TaskConfiguration.Descriptor.Outputs[cfg.Key].Destination), cfg.Value);
            }

            // todo: pragma.
            return result;
        }

        protected IDictionary<string, string> BuildConfiguration(IConfigurationCollection configuration,
            IEnumerable<(IInputTemplate, IInputMapping)> inputTemplates)
        {
            // build the configuration
            IDictionary<string, string> configurations = new Dictionary<string, string>();
            foreach (var output in configuration.Descriptor.Outputs.Values)
            {
                var sectionBuilder = new StringBuilder();
                var serializer = new KeyValuePairConfigurationSerializer(output.BooleanMapping, "nul", "=");
                foreach (var section in configuration.Where(c => configuration.Descriptor.GetDestination(c.Key) == output.Key))
                {
                    sectionBuilder.Append(serializer.Serialize(section.Value));
                }

                configurations[output.Key] = sectionBuilder.ToString();
            }

            var retroarchSerializer = new KeyValuePairConfigurationSerializer(configuration.Descriptor.Outputs["#retroarch"].BooleanMapping, "nul", "=");

            IInputSerializer inputSerializer = new InputSerializer(retroarchSerializer);

            // handle input config
            foreach ((IInputTemplate template, IInputMapping mapping) in inputTemplates)
            {
                if (mapping == null)
                {
                    throw new InvalidOperationException("Adapter does not support device layout");
                }

                // serialize the input template
                var inputConfig = inputSerializer.Serialize(template, mapping);
                configurations["#retroarch"] += inputConfig;
            }

            return configurations;
        }
    }
}
