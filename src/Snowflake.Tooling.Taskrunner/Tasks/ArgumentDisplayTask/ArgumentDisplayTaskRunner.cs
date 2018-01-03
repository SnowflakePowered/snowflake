using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;
using Snowflake.Tooling.Taskrunner.Framework.Parser;
using Snowflake.Tooling.Taskrunner.Framework.Tasks;

namespace Snowflake.Tooling.Taskrunner.Tasks.ArgumentDisplayTask
{
    [Task("display", "[Debug] Displays the parsed arguments using the argument parser.")]
    public class ArgumentDisplayTaskRunner : TaskRunner<EmptyArguments>
    {
        public override async Task<int> Execute(EmptyArguments arguments, string[] args)
        {
            var parser = new ArgumentParser();
            var posArgs = parser.ParsePositionalArguments(args);
            var namedArgs = parser.ParseNamedArguments(args);
            Console.WriteLine("== Named Arguments ==");
            foreach (KeyValuePair<string, string> namedArg in namedArgs)
            {
                Console.WriteLine($"{namedArg.Key}: {namedArg.Value}");
            }

            Console.WriteLine("== Positional Arguments ==");
            foreach (KeyValuePair<int, string> posArg in posArgs)
            {
                Console.WriteLine($"{posArg.Key}: {posArg.Value}");
            }

            return 0;
        }
    }
}
