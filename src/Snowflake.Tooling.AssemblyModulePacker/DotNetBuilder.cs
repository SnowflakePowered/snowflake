using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CliWrap;
using System.Linq;
using System.Threading.Tasks;

namespace Snowflake.Tooling.AssemblyModulePacker
{
    internal class DotNetBuilder
    {
        private readonly Cli dotnetProcess;
        private readonly FileInfo projectFile;
        private readonly ModuleDefinition moduleDefinition;
        private readonly IEnumerable<string> args;
        public DotNetBuilder(ModuleDefinition moduleDefinition, FileInfo projectFile, IEnumerable<string> args)
        {
            this.dotnetProcess = new Cli("dotnet", projectFile.DirectoryName);
            this.projectFile = projectFile;
            this.moduleDefinition = moduleDefinition;
            this.args = args;
        }

        public DirectoryInfo Clean()
        {
            var outputDirectory = projectFile.Directory
                           .CreateSubdirectory("bin")
                           .CreateSubdirectory("module");
            try
            {
                foreach(var directory in outputDirectory.EnumerateDirectories())
                {
                    directory.Delete(true);
                }
            }
            catch (IOException ex)
            {
                throw new IOException("Unable to clean output directory, is it in use?", ex);
            }
            var moduleRoot = outputDirectory
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(moduleDefinition.Entry));
            return moduleRoot.CreateSubdirectory("contents");
        }

        public async Task<DirectoryInfo> Build()
        {
            var moduleContents = this.Clean();
            var dotnetArgs = this.args.Prepend(moduleContents.FullName)
                .Prepend("-o")
                .Prepend(projectFile.FullName)
                .Prepend("publish");
            var output = await this.dotnetProcess.ExecuteAsync(String.Join(" ", dotnetArgs));
            Console.WriteLine(output.StandardOutput);
            return this.CopyModuleFile(moduleContents).Directory;
        }

        public FileInfo CopyModuleFile(DirectoryInfo contentsDirectory)
        {
            var moduleFile = contentsDirectory.EnumerateFiles().FirstOrDefault(f => f.Name == "module.json");
            return moduleFile.CopyTo(Path.Combine(moduleFile.Directory.Parent.FullName, moduleFile.Name)); 
        }

    }
}
