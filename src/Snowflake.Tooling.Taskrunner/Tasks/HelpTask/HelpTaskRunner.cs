using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Tooling.Taskrunner.Framework;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;
using Snowflake.Tooling.Taskrunner.Framework.Tasks;

namespace Snowflake.Tooling.Taskrunner.Tasks.HelpTask
{
    [Task("help", "Displays help for the given task.")]
    public class HelpTaskRunner : TaskRunner<HelpTaskArguments>
    {
        private VerbContainer Verbs { get; }
        public HelpTaskRunner(VerbContainer container)
        {
            this.Verbs = container;
        }

        public override async Task<int> Execute(HelpTaskArguments arguments, string[] args)
        {
            if (!this.Verbs.Contains(arguments.Task))
            {
                Console.WriteLine($"Unknown task '{arguments.Task}'.");
                return 1;
            }

            var taskArgType = this.Verbs[arguments.Task].ArgumentType;
            Console.WriteLine(this.Verbs[arguments.Task].Description);
            Console.WriteLine();
            var namedArgs = (from prop in taskArgType.GetProperties()
                            let attr = prop.GetCustomAttribute<NamedArgumentAttribute>()
                            where attr != null
                            select (attr, prop.Name, prop.PropertyType)).ToList();

            var posArgs = (from prop in taskArgType.GetProperties()
                           let attr = prop.GetCustomAttribute<PositionalArgumentAttribute>()
                           where attr != null
                           orderby attr.Position
                           select (attr, prop.Name)).ToList();

            Console.Write($"Usage: dotnet snowflake {arguments.Task}");

            foreach((var attr, var propName) in posArgs)
            {
                Console.Write($" <{propName.ToUpperInvariant()}> ");
            }
               
            if (namedArgs.Count != 0)
            {
                Console.Write(" [options]");
            }

            Console.WriteLine("\n");

            if (posArgs.Count != 0)
            {
                Console.WriteLine("Arguments: ");
                foreach ((var attr, var propName) in posArgs)
                {
                    Console.WriteLine($"{$"<{propName.ToUpperInvariant()}>".PadLeft(10).PadRight(10)} {attr.Description}");
                }
            }

            Console.WriteLine("\n");

            if (namedArgs.Count != 0)
            {
                Console.WriteLine("Options: ");
                foreach ((var attr, var propName, var propType) in namedArgs)
                {
                    Console.Write($"-{attr.ShortName}, --{attr.LongName}");

                    if (propType != typeof(bool))
                    {
                        Console.Write($" <{propName}> ");
                    }

                    Console.Write($"{attr.Description.PadLeft(attr.Description.Length + 10)}");
                    Console.WriteLine();
                }
            }

            return 0;
        }
    }
}
