using System;
using System.IO;
using System.Linq;
using System.Xml;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath OutputDirectory => GetOutDirectory();

    AbsolutePath Tooling => SourceDirectory / "Snowflake.Tooling.Taskrunner";
    AbsolutePath Tests => SourceDirectory / "Snowflake.Framework.Tests";

    AbsolutePath BuildDirectory => RootDirectory / "build";
    AbsolutePath CoveragePath => BuildDirectory / "coverage";

    AbsolutePath ToolingDirectory => BuildDirectory / "snowflake-cli";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {

            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target BuildTooling => _ => _
       .DependsOn(Restore)
       .Executes(() =>
       {
           DotNetBuild(s => s
               .SetProjectFile(Tooling)
               .SetOutputDirectory(ToolingDirectory));
       });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Tests)
                .SetLogger("trx"));
        });

    Target PackModules => _ => _
        .DependsOn(Compile)
        .DependsOn(BuildTooling)
        .Executes(() =>
        {
            EnsureExistingDirectory(OutputDirectory);

            foreach (Project p in Solution.GetProjects("Snowflake.*"))
            {
                if (!GetSdkVersion(p).StartsWith("Snowflake.Framework.Sdk")) continue;
                if (!File.Exists((p.Directory / "module.json"))) continue;

                DotNet("snowflake build", p.Directory);
                DotNet($"snowflake pack \"./bin/module/{p.Name}\" -o \"{OutputDirectory}\"", p.Directory);
            }
        });

    Target PackFramework => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            EnsureExistingDirectory(OutputDirectory);
            foreach (Project p in Solution.GetProjects("Snowflake.Framework*"))
            {
                DotNetPack(s => s
                    .SetProject(p.Path)
                    .SetOutputDirectory(OutputDirectory)
                    .SetVersionSuffix($"alpha.{GetBuildNumber()}"));
            }
        });

    Target Bootstrap => _ => _
        .DependsOn(PackModules)
        .DependsOn(BuildTooling)
        .Executes(() =>
        {
            DotNet($"{ToolingDirectory / "dotnet-snowflake.dll"} install-all -d {OutputDirectory}");
        });

    Target ContinuousIntegration => _ => _
        .DependsOn(Test)
        .DependsOn(PackModules)
        .DependsOn(PackFramework);

    static string GetBuildNumber()
    {
        string buildNumber = EnvironmentInfo.Variable("BUILD_NUMBER");
        if (String.IsNullOrWhiteSpace(buildNumber))
        {
            return "dirty";
        }
        return buildNumber;
    }

    static AbsolutePath GetOutDirectory()
    {
        string staging = EnvironmentInfo.Variable("STAGING");
        if (String.IsNullOrWhiteSpace(staging))
        {
            return RootDirectory / "out";
        }
        return (AbsolutePath)staging;
    }

    static string GetSdkVersion(Project p)
    {
        using (XmlReader reader = XmlReader.Create(p.Path))
        {
            if (reader.MoveToContent() == XmlNodeType.Element && reader.HasAttributes)
            {
                if (reader.MoveToAttribute("Sdk"))
                {
                    return reader.ReadContentAsString();
                }
            }
        }
        return String.Empty;
    }
}
