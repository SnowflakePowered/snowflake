using System.Collections.Generic;
using CommandLine;

namespace Snowflake.Packaging
{

  
    [Verb("make", HelpText = "Make a snowball package")]
    internal class MakePackageOptions
    {
        [Value(0, 
           HelpText = "The file to make the snowball package for. This can either be a Plugin .dll file, a .emulatordef Emulator Assembly Definition, or a Theme directory",
           Required = true)]
        public string FileName { get; set; }

        [Option('i', "info",
            HelpText = "Specify the package info file.")]
        public string PackageInfoFile { get; set; }

        [Option('p', "plugin",
            SetName = "Make Type", 
            HelpText = "Make a snowball for a plugin. " +
                       "A plugin is expected to have an EMBEDDED snowball.json, " +
                       "and be in the format of a .dll file. " +
                       "Any files in the plugin's resource folder will also be copied",
            Required = false)]
        public bool MakePlugin { get; set; }

        [Option('e', "emulator",
            SetName = "Make Type", 
            HelpText = "Make a snowball for an emulator assembly. " +
                       "An emulator assembly is expected to have contents in a folder named the ID, " +
                       "and include a snowball.json inside the folder. " +
                       "The input file is expected to be a format of an .emulatordef file describing the assembly.",
            Required = false)]
        public bool MakeEmulator { get; set; }

        [Option('t', "theme",
            SetName = "Make Type",
            HelpText = "Make a snowball for a theme. " +
                       "A theme is expected to be in the format of a folder containing the theme contents, " +
                       "and include a snowball.json and a theme.json inside the folder.", 
            Required = false)]
        public bool MakeTheme { get; set; }

        [Option('n', "nuget",
        HelpText = "Sign and wrap the resulting package instead of returning the package",
        Required = false)]
        public bool WrapNuget { get; set; }
    

        [Option('o', "output", HelpText = "The output directory. Defaults to current working directory",
            Required = false)]
        public string OutputDirectory { get; set; }

    }

    [Verb("install", HelpText = "Install a snowball package")]
    internal class InstallOptions
    {
        [Value(0, HelpText = "The package id or snowball to install", Required = true)]
        public string Package { get; set; }

        [Option('r', "remote", HelpText = "Install a remote package", Default = true)]
        public bool RemoteInstall { get; set; }

        [Option('l', "local", HelpText = "Install a local packaged .snowball package")]
        public bool LocalInstall { get; set; }
    }

    [Verb("uninstall", HelpText = "Uninstall a snowball package")]
    internal class UninstallOptions
    {
        [Value(0, HelpText = "The package id to uninstall", Required = true)]
        public string PackageId { get; set; }
    }

    [Verb("publish", HelpText = "Publish a snowball package for approval")]
    internal class PublishOptions 
    {
        [Value(0,
          HelpText = "The package id of the built snowball, or the full filename of the snowball if (--fullpath), or The file to make the snowball package for if (--make)",
          Required = true)]
        public string FileName { get; set; }

        [Option('i', "info",
            HelpText = "Specify the package info file. (--make only)")]
        public string PackageInfoFile { get; set; }

        [Option('p', "plugin",
            SetName = "Make Type",
            HelpText = "(--make only) Make a snowball for a plugin. " +
                       "A plugin is expected to have an EMBEDDED snowball.json, " +
                       "and be in the format of a .dll file. " +
                       "Any files in the plugin's resource folder will also be copied",
            Required = false)]
        public bool MakePlugin { get; set; }

        [Option('e', "emulator",
            SetName = "Make Type",
            HelpText = "(--make only) Make a snowball for an emulator assembly. " +
                       "An emulator assembly is expected to have contents in a folder named the ID, " +
                       "and include a snowball.json inside the folder. " +
                       "The input file is expected to be a format of an .emulatordef file describing the assembly.",
            Required = false)]
        public bool MakeEmulator { get; set; }

        [Option('t', "theme",
            SetName = "Make Type",
            HelpText = "(--make only) Make a snowball for a theme. " +
                       "A theme is expected to be in the format of a folder containing the theme contents, " +
                       "and include a snowball.json and a theme.json inside the folder.",
            Required = false)]
        public bool MakeTheme { get; set; }

        [Option('m', "make", HelpText = "Make a package rather than publish an existing one")]
        public bool MakePackage { get; set; }

        [Option('s', "snowball", HelpText = "Publish an existing package", Default = true)]
        public bool Prebuilt { get; set; }

        [Option('f', "fullpath", HelpText = "Specify the full path to the package", Default = false)]
        public bool FullPath { get; set; }

        [Option('r', "retry", HelpText = "Sets the amount of retries", Default= 3)]
        public int RetryCount { get; set; }

        [Option('t', "timeout", HelpText = "Timeout when uploading to NuGet in seconds", Default = 300)]
        public int Timeout { get; set; }

        [Option('g', "gh-only", HelpText = "Only publish github index.")]
        public bool GithubOnly { get; set; }


    }

    [Verb("auth", HelpText = "Setup to publish. Requires a GitHub account.")]
    internal class AuthOptions
    {
        [Option("nuget", HelpText = "Your NuGet API Key", Required = true)]
        public string NuGetAPIKey { get; set; }

        [Option("gh-user", HelpText = "Your GitHub username", Required = true)]
        public string GithubUser { get; set; }

        [Option("gh-pass", HelpText = "Your GitHub password", Required = true)]
        public string GithubPassword { get; set; }

        [Option("gh-auth", HelpText = "Your GitHub 2-factor authentication code", Required = false, Default = "")]
        public string GitHub2FA { get; set; }
    }
}