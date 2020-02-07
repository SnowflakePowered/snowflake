using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Snowflake.Tooling.Taskrunner.Tasks.AssemblyModuleBuilderTasks
{
    internal class DotNetBuilder
    {
        private readonly ProcessStartInfo dotnetProcess;
        private readonly FileInfo projectFile;
        private readonly ModuleDefinition moduleDefinition;
        private readonly IEnumerable<string> args;
        private readonly string outputDirectory;
        private readonly bool release;

        public DotNetBuilder(ModuleDefinition moduleDefinition, FileInfo projectFile, string outputDirectory,
            bool release,
            string args)
        {
            this.dotnetProcess = new ProcessStartInfo()
            {
                FileName = "dotnet",
                WorkingDirectory = projectFile.DirectoryName
            };
            this.projectFile = projectFile;
            this.moduleDefinition = moduleDefinition;
            this.outputDirectory = outputDirectory;
            this.release = release;
            this.args = args.Split(' ');
        }

        public async Task Build()
        {
            var dotnetArgs = (this.outputDirectory != null ? this.args.Prepend($"/p:_SnowflakeModuleOutDir={this.outputDirectory}") : this.args)
                .Prepend(this.release ? "Release-Module" : "Debug-Module")
                .Prepend("-c")
                .Prepend($"\"{projectFile.FullName}\"")
                .Prepend("publish");
            this.dotnetProcess.Arguments = String.Join(" ", dotnetArgs);
            this.dotnetProcess.RedirectStandardOutput = true;
            var output = Process.Start(this.dotnetProcess);
            await Task.Run(() =>
            {
                output.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
                output.BeginOutputReadLine();
                output.WaitForExit();
            });
            if (output.ExitCode != 0)
                throw new InvalidOperationException(
                    "Unable to build module; MSBuild was unable to build your assembly.");
        }
    }
}
