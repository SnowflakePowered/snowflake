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

        public DotNetBuilder(ModuleDefinition moduleDefinition, FileInfo projectFile, string outputDirectory,
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
            this.args = args.Split(' ');
        }

        public DirectoryInfo Clean()
        {
            var outputDirectory = this.outputDirectory != null
                ? new DirectoryInfo(Path.GetFullPath(this.outputDirectory))
                    .CreateSubdirectory(Path.GetFileNameWithoutExtension(moduleDefinition.Entry))
                : projectFile.Directory
                    .CreateSubdirectory("bin")
                    .CreateSubdirectory("module")
                    .CreateSubdirectory(Path.GetFileNameWithoutExtension(moduleDefinition.Entry));

            try
            {
                foreach (var dir in outputDirectory.EnumerateDirectories())
                {
                    dir.Delete(true);
                }

                foreach (var file in outputDirectory.EnumerateFiles())
                {
                    file.Delete();
                }
            }
            catch (IOException ex)
            {
                throw new IOException("Unable to clean output directory, is it in use?", ex);
            }

            return outputDirectory.CreateSubdirectory("contents");
        }

        public async Task<DirectoryInfo> Build()
        {
            var moduleContents = this.Clean();
            var dotnetArgs = this.args
                .Prepend(moduleContents.FullName)
                .Prepend("-o")
                .Prepend(projectFile.FullName)
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
            return this.CopyModuleFile(moduleContents).Directory;
        }

        public FileInfo CopyModuleFile(DirectoryInfo contentsDirectory)
        {
            var moduleFile = contentsDirectory.EnumerateFiles().FirstOrDefault(f => f.Name == "module.json");
            return moduleFile.CopyTo(Path.Combine(moduleFile.Directory.Parent.FullName, moduleFile.Name));
        }
    }
}
