using CommandLine;
using Snowflake.Tooling.RestClient.Commands;
using Snowflake.Tooling.RestClient.Commands.Game;
using System;
using System.Linq;

namespace Snowflake.Tooling.RestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var discovery = new CommandDiscovery();
            CommandLine.Parser.Default.ParseArguments(args, 
                discovery.Types.Select(t => t.optionType).ToArray())
                .MapResult(t => discovery.Execute(t), err => 1);

        }
    }
}
