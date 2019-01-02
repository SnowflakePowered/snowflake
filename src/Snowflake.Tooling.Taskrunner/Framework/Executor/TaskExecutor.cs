using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Snowflake.Tooling.Taskrunner.Framework.Parser;
using Snowflake.Tooling.Taskrunner.Framework.Tasks;
using System.Reflection;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;
using System.ComponentModel;

namespace Snowflake.Tooling.Taskrunner.Framework.Executor
{
    public class TaskExecutor
    {
        public TaskContainer Verbs { get; }
        public ArgumentParser Parser { get; }

        public TaskExecutor(TaskContainer verbContainer, ArgumentParser parser)
        {
            this.Verbs = verbContainer;
            this.Parser = parser;
        }

        public VerbTaskResult ExecuteTask(string verb, string[] args)
        {
            if (!this.Verbs.Contains(verb))
            {
                Console.Write($"'{verb}' is not a valid task. ");
                this.Verbs["list"].Execute(null, null);
                return 1;
            }

            var task = this.Verbs[verb];
            var namedArgs = this.Parser.ParseNamedArguments(args);
            var posArgs = this.Parser.ParsePositionalArguments(args);
            var typedArguments = Instantiate.CreateInstance(task.ArgumentType);


            foreach (KeyValuePair<string, string> namedArg in namedArgs)
            {
                foreach ((var prop, var converted) in from prop in task.ArgumentType.GetProperties()
                    let attr = prop.GetCustomAttribute<NamedArgumentAttribute>()
                    where attr != null
                    where attr.LongName.Equals(namedArg.Key, StringComparison.OrdinalIgnoreCase)
                          || attr.ShortName.Equals(namedArg.Key, StringComparison.OrdinalIgnoreCase)
                    let converter = TypeDescriptor.GetConverter(prop.PropertyType)
                    where converter != null
                    let converted = converter.ConvertFromInvariantString(namedArg.Value)
                    select (prop, converted)
                )
                {
                    prop.SetValue(typedArguments, converted);
                }
            }

            foreach (KeyValuePair<int, string> posArg in posArgs)
            {
                foreach ((var prop, var converted) in from prop in task.ArgumentType.GetProperties()
                    let attr = prop.GetCustomAttribute<PositionalArgumentAttribute>()
                    where attr != null
                    where attr.Position == posArg.Key
                    let converter = TypeDescriptor.GetConverter(prop.PropertyType)
                    where converter != null
                    let converted = converter.ConvertFromInvariantString(posArg.Value)
                    select (prop, converted)
                )
                {
                    prop.SetValue(typedArguments, converted);
                }
            }

            return task.Execute(typedArguments, args);
        }
    }
}
