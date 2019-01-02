using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Tooling.Taskrunner.Framework;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;
using Snowflake.Tooling.Taskrunner.Framework.Tasks;

namespace Snowflake.Tooling.Taskrunner.Tasks.TaskListTask
{
    [Task("list", "Lists the available tasks.")]
    public class TaskListTaskRunner : TaskRunner<EmptyArguments>
    {
        private TaskContainer Verbs { get; }

        public TaskListTaskRunner(TaskContainer container)
        {
            this.Verbs = container;
        }

        public override async Task<int> Execute(EmptyArguments arguments, string[] args)
        {
            Console.WriteLine("The following tasks are available.");
            Console.WriteLine("Tasks:");
            foreach (var task in this.Verbs)
            {
                Console.WriteLine($"{task.Name.PadLeft(task.Name.Length + 2).PadRight(20)} {task.Description}");
            }

            return 0;
        }
    }
}
