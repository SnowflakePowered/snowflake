#addin "Cake.Incubator"
#tool nuget:?package=JetBrains.dotCover.CommandLineTools&version=2018.3.4
#tool nuget:?package=Codecov&version=1.3.0
#addin nuget:?package=Cake.Codecov&version=0.5.0
var target = Argument("target", "Default");

Task("Default")
  .Does(() =>
  {
    DotNetCoreRestore("../src/Snowflake.sln");
    DotNetCoreBuild("../src/Snowflake.sln");
  });

Task("BuildTooling")
  .Does(() => {
    DotNetCoreBuild("../src/Snowflake.Tooling.Taskrunner", new DotNetCoreBuildSettings(){
      OutputDirectory = "./snowflake-cli"
    });
  });

Task("CreateArtifactsOutputDirectory")
  .Does(() => {
    CreateDirectory("out");
  });

Task("PackFrameworkNuget")
  .IsDependentOn("Default")
  .DoesForEach(ParseSolution(new FilePath("../src/Snowflake.sln"))
               .Projects
               .Where(p => p.Name.StartsWith("Snowflake.Framework")), project => {
    DotNetCorePack(project.Path.FullPath, new DotNetCorePackSettings() {
      OutputDirectory = "out",
    });
  });


Task("PackFrameworkNugetAppveyor")
  .IsDependentOn("Default")
  .DoesForEach(ParseSolution(new FilePath("../src/Snowflake.sln"))
               .Projects
               .Where(p => p.Name.StartsWith("Snowflake.Framework")), project => {
    DotNetCorePack(project.Path.FullPath, new DotNetCorePackSettings() {
      OutputDirectory = "out",
      VersionSuffix = $"alpha.{EnvironmentVariable("APPVEYOR_BUILD_NUMBER")}"
    });
  });

Task("PackModules")
  .IsDependentOn("Default")
  .IsDependentOn("CreateArtifactsOutputDirectory")
  .DoesForEach(ParseSolution(new FilePath("../src/Snowflake.sln"))
               .Projects, project => {
    var projectProps = ParseProject(project.Path, "debug");
    if (projectProps.NetCore?.Sdk.StartsWith("Snowflake.Framework.Sdk/") == true && FileExists(project.Path.GetDirectory().CombineWithFilePath("module.json"))) {
        Information($"Building {projectProps.AssemblyName}");
        DotNetCoreTool(project.Path, "snowflake", "build");
        Information($"Packing module..");
        DotNetCoreTool(project.Path, "snowflake", new ProcessArgumentBuilder()
            .Append("pack")
            .AppendQuoted($"./bin/module/{projectProps.AssemblyName}")
            .Append("-o")
            .AppendQuoted($"{Environment.CurrentDirectory}/out"));

    }
  });

Task("Test")
  .IsDependentOn("Default")
  .Does(() => {
    DotNetCoreTest("../src/Snowflake.Framework.Tests");
  });

Task("AnalyseHtml")
  .Does(() => {
    DotCoverAnalyse(tool => {
        tool.DotNetCoreTest("../src/Snowflake.Framework.Tests", new DotNetCoreTestSettings(){
        });
      },
      new FilePath("./coverage/coverage.html"),
      new DotCoverAnalyseSettings()
      {
        ReportType = DotCoverReportType.HTML,
      }
      .WithFilter("+:module=Snowflake.Framework*")
      .WithFilter("+:module=Snowflake.Support*")
      .WithFilter("-:module=Snowflake.Framework.Tests*")
      .WithFilter("-:module=Snowflake.Support.Caching.KeyedImageCache")
      .WithFilter("-:module=Snowflake.Support.FileSignatures")
      .WithFilter("-:module=Snowflake*;class=*Composable")
    );
  });

  
Task("AnalyseXml")
  .Does(() => {
    DotCoverAnalyse(tool => {
        tool.DotNetCoreTest("../src/Snowflake.Framework.Tests", new DotNetCoreTestSettings(){
        });
      },
      new FilePath("./coverage/coverage.xml"),
      new DotCoverAnalyseSettings()
      {
        ReportType = DotCoverReportType.DetailedXML,
      }
      .WithFilter("+:module=Snowflake.Framework*")
      .WithFilter("+:module=Snowflake.Support*")
      .WithFilter("-:module=Snowflake.Framework.Tests*")
      .WithFilter("-:module=Snowflake.Support.Caching.KeyedImageCache")
      .WithFilter("-:module=Snowflake*;class=*Composable")
    );
  });

Task("Codecov")
  .IsDependentOn("AnalyseXml")
  .Does(() => {
    Codecov("./coverage/coverage.xml");
  });

Task("Bootstrap")
  .IsDependentOn("PackModules")
  .IsDependentOn("BuildTooling")
  .Does(() => {
    DotNetCoreExecute("./snowflake-cli/dotnet-snowflake.dll", $"install-all -d ./out");
  });

Task("Appveyor")
  .IsDependentOn("Default")
  .IsDependentOn("Codecov")
  .IsDependentOn("PackModules")
  .IsDependentOn("PackFrameworkNugetAppveyor");

Task("Travis")
  .IsDependentOn("Default")
  .IsDependentOn("Test")
  .IsDependentOn("PackModules");
  
RunTarget(target);
