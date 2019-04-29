using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using Snowflake.Tooling.Taskrunner.Framework;
using Snowflake.Tooling.Taskrunner.Framework.Executor;
using Snowflake.Tooling.Taskrunner.Framework.Parser;
using Snowflake.Tooling.Taskrunner.Tasks.ArgumentDisplayTask;
using Snowflake.Tooling.Taskrunner.Tasks.AssemblyModuleBuilderTask;
using Snowflake.Tooling.Taskrunner.Tasks.HelpTask;
using Snowflake.Tooling.Taskrunner.Tasks.InstallAllTask;
using Snowflake.Tooling.Taskrunner.Tasks.InstallTask;
using Snowflake.Tooling.Taskrunner.Tasks.PackTask;
using Snowflake.Tooling.Taskrunner.Tasks.TaskListTask;
using System.Diagnostics;

namespace Snowflake.Tooling.Taskrunner
{
    public class Program
    {
        static void Main(string[] args)
        {
            var parser = new ArgumentParser();
            var container = new TaskContainer();
            var executor = new TaskExecutor(container, parser);
            container.Add(new HelpTaskRunner(container));
            container.Add(new TaskListTaskRunner(container));
            container.Add(new ArgumentDisplayTaskRunner());
            container.Add(new AssemblyModuleBuilderTaskRunner());
            container.Add(new PackTaskRunner());
            container.Add(new InstallTaskRunner());
            container.Add(new InstallAllTaskRunner());

            Stopwatch timer = new Stopwatch();
            timer.Start();
            if (args.Length == 0)
            {
                executor.ExecuteTask("list", args);
                Environment.Exit(0);
            }

            var result = executor.ExecuteTask(args[0], args.Skip(1).ToArray());
            foreach (var exception in result.RaisedExceptions)
            {
                Console.WriteLine(exception.Message);
            }

            timer.Stop();
            Console.WriteLine($"Completed in {timer.Elapsed.ToString("c")}.");
            Environment.Exit(result.ExitCode);
        }
    }
}
