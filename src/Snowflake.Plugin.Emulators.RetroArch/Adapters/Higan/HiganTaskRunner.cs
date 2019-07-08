using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Execution.Extensibility;
using Snowflake.Execution.Process;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Filesystem;
using Snowflake.Input.Device;
using Snowflake.Model.Records.File;

namespace Snowflake.Adapters.Higan
{
    public class HiganTaskRunner : IEmulatorTaskRunner
    {
        public IEmulatorExecutable RetroArchExecutable { get; }
        public IEmulatorTaskRootDirectoryProvider DirectoryProvider { get; }
        public IDirectory CoreDirectory { get; }
        public IEmulatorProperties Properties { get; }

        public HiganTaskRunner(IEmulatorExecutable retroArchExecutable,
            IPluginProvision pluginProvision,
            IEmulatorProperties properties)
        {
            this.RetroArchExecutable = retroArchExecutable;
            if (this.RetroArchExecutable == null)
            {
                throw new FileNotFoundException("RetroArch emulator executable was not installed!");
            }

            this.Properties = properties;
            this.CoreDirectory = pluginProvision.CommonResourceDirectory.OpenDirectory("cores");
        }

        public async Task<IEmulatorTaskResult> ExecuteEmulationAsync(IEmulatorTask task)
        {
            throw new NotImplementedException();
            // IFileRecord fileToExecute = task.EmulatingGame.WithFiles().FirstOrDefault(f => this.Properties.Mimetypes.Contains(f.MimeType));
            //IFileRecord fileToExecute = null;
            //if (fileToExecute == null)
            //{
            //    throw new FileNotFoundException(
            //        $"Unable to find a compatible ROM for game {task.EmulatingGame.RecordId}.");
            //}

            //IEmulatorTaskResult result = new RetroArchTaskResult(task.ProcessTaskRoot, task.GameSaveLocation);
            //IProcessBuilder builder = this.RetroArchExecutable.GetProcessBuilder();
            //builder.WithArgument("--verbose")
            //    .WithArgument("-s", task.ProcessTaskRoot.SaveDirectory.FullName)
            //    .WithArgument("-c", Path.Combine(task.ProcessTaskRoot.ConfigurationDirectory.FullName, "retroarch.cfg"))
            //    .WithArgument("-L", Path.Combine(this.CoreDirectory.FullName, task.Pragmas["retroarch_core"]));

            ////  .WithArgument(fileToExecute.);

            //foreach (var cfg in this.BuildConfiguration(task.EmulatorConfiguration, task.ControllerConfiguration))
            //{
            //    await File.WriteAllTextAsync(Path.Combine(task.ProcessTaskRoot.ConfigurationDirectory.FullName,
            //        task.EmulatorConfiguration.Descriptor.Outputs[cfg.Key].Destination), cfg.Value);
            //}

            //var psi = builder.ToProcessStartInfo();
            //var process = Process.Start(psi);
            //process.Exited += (s, e) => result.Closed();
            //return result;
        }

        protected IDictionary<string, string> BuildConfiguration(IConfigurationCollection configuration,
            IEnumerable<(IInputTemplate, IInputMapping)> inputTemplates)
        {
            throw new NotImplementedException();
            //    // build the configuration
            //    IDictionary<string, string> configurations = new Dictionary<string, string>();
            //    foreach (var output in configuration.Descriptor.Outputs.Values)
            //    {
            //        var sectionBuilder = new StringBuilder();
            //        var serializer = new KeyValuePairConfigurationSerializer(output.BooleanMapping, "nul", "=");
            //        foreach (var section in configuration.Where(c =>
            //            configuration.Descriptor.GetDestination(c.Key) == output.Key))
            //        {
            //            sectionBuilder.Append(serializer.Serialize(section.Value));
            //        }

            //        configurations[output.Key] = sectionBuilder.ToString();
            //    }

            //    var retroarchSerializer =
            //        new KeyValuePairConfigurationSerializer(configuration.Descriptor.Outputs["#retroarch"].BooleanMapping,
            //            "nul", "=");

            //    IInputSerializer inputSerializer = new InputSerializer(retroarchSerializer);

            //    // handle input config
            //    foreach ((IInputTemplate template, IInputMapping mapping) in inputTemplates)
            //    {
            //        if (mapping == null)
            //        {
            //            throw new InvalidOperationException("Adapter does not support device layout");
            //        }

            //        // serialize the input template
            //        var inputConfig = inputSerializer.Serialize(template, mapping);
            //        configurations["#retroarch"] += inputConfig;
            //    }

            //    return configurations;
            //}
        }
    }
}
