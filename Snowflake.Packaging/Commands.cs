using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.Reflection;
namespace Snowflake.Packaging
{
    [Verb("sign", HelpText = "Sign a snowball generating public key and signature")]
    internal class SignOptions
    {
        [Value(0, HelpText = "File to sign", Required = true)]
        public string FileName { get; set; }
    }
    [Verb("verify", HelpText = "Sign a snowball generating public key and signature")]
    internal class VerifyOptions
    {
        [Value(0, HelpText = "File to verify", Required = true)]
        public string FileName { get; set; }
    }
    [Verb("pack", HelpText = "Pack a folder into a snowball package")]
    internal class PackOptions
    {
        [Value(0, HelpText ="The snowball package root with a snowball folder and a snowball.json file", Required = true)]
        public string PluginRoot { get; set; }
        [Value(1, HelpText = "The output directory. Defaults to current working directory", Required = false)]
        public string OutputDirectory { get; set; }
    }

    [Verb("make-plugin", HelpText = "Make a snowball package for a plugin")]
    internal class MakePluginOptions
    {
        [Option('p', "plugin", HelpText = "The path to the main plugin dll.", Required = true)]
        public string PluginFile { get; set; }
        [Option('i', "info", HelpText = "Specify the package info file. The plugin must have a snowball.json in it's embedded resources otherwise", Required = false)]
        public string PackageInfoFile { get; set; }
        [Option('o', "output", HelpText = "The output directory. Defaults to current working directory", Required = false)]
        public string OutputDirectory { get; set; }
        [Option('b', "bare", HelpText = "Do not pack the plugin, return the raw directory instead")]
        public bool Bare { get; set; }
       
    }
    [Verb("make-theme", HelpText = "Make a snowball package for a theme")]
    internal class MakeThemeOptions
    {
        [Option('t', "theme", HelpText = "The path to the theme root. Must have a theme.json inside", Required = true)]
        public string ThemeDirectory { get; set; }
        [Option('i', "info", HelpText = "Specify the package info file. The theme must have a snowball.json in it's theme root otherwise")]
        public string PackageInfoFile { get; set; }
        [Option('o', "output", HelpText = "The output directory. Defaults to current working directory", Required = false)]
        public string OutputDirectory { get; set; }
        [Option('b', "bare", HelpText = "Do not pack the plugin, return the raw directory instead")]
        public bool Bare { get; set; }
    }
    [Verb("make-emulator", HelpText = "Make a snowball package for an emulator assembly")]
    internal class MakeEmulatorAssemblyOptions
    {
        [Option('d', "definition", HelpText = "The path to the emulatordef file. Must have emulator files beside this.", Required = true)]
        public string EmulatorDefinitionFile { get; set; }
        [Option('i', "info", HelpText = "Specify the package info file. The emulator assembly folder must have a snowflake.json otherwise")]
        public string PackageInfoFile { get; set; }
        [Option('o', "output", HelpText = "The output directory. Defaults to current working directory", Required = false)]
        public string OutputDirectory { get; set; }
        [Option('b', "bare", HelpText = "Do not pack the plugin, return the raw directory instead")]
        public bool Bare { get; set; }

    }

    [Verb("install", HelpText = "Install a snowball package")]
    internal class InstallOptions
    {
        [Value(0, HelpText = "The package file or id", Required = true)]
        public string PackageFile { get; set; }
        [Value(1, HelpText = "The snowflake root. Defaults to %appdata%/snowflake", Required = false)]
        public string SnowflakeRoot { get; set; }
        [Option('l', "local", HelpText = "Treat the first argument as a file path to install a local package")]
        public bool LocalInstall { get; set; }
    }
    
    [Verb("uninstall", HelpText = "Uninstall a snowball package")]
    internal class UninstallOptions
    {
        [Value(0, HelpText = "The package id to install", Required = true)]
        public string PackageId { get; set; }
        [Value(1, HelpText = "The snowflake root. Defaults to %appdata%/snowflake", Required = false)]
        public string SnowflakeRoot { get; set; }

    }

    [Verb("publish", HelpText = "Publish a snowball package for approval")]
    internal class PublishOptions
    {
        [Value(0, HelpText = "The package to publish", Required = true)]
        public string PackageFile { get; set; }
    }

    [Verb("setup", HelpText = "Setup to publish. Requires a GitHub account.")]
    internal class SetupOptions
    {
        [Option("nuget", HelpText = "Your NuGet API Key", Required = true)]
        public string NuGetAPIKey { get; set; }
        [Option("gh-user", HelpText = "Your NuGet API Key", Required = true)]
        public string GithubUser { get; set; }
        [Option("gh-pass", HelpText = "Your NuGet API Key", Required = true)]
        public string GithubPassword { get; set; }


    }

}
