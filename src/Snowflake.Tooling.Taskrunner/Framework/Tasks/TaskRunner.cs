using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;

namespace Snowflake.Tooling.Taskrunner.Framework.Tasks
{
    public abstract class TaskRunner<T> : ITaskRunner<T> where T: class, new()
    {
        public string Name { get; }

        public string Description { get; }

        public Type ArgumentType => typeof(T);

        public TaskRunner()
        {
            var taskAttr = this.GetType().GetCustomAttribute<TaskAttribute>();
            this.Name = taskAttr.Name;
            this.Description = taskAttr.Description;
        }

        public VerbTaskResult Execute(object arguments, string[] args)
        {
            try
            {
                return this.Execute((T)arguments, args).Result;
            }
            catch (AggregateException e)
            {
                return new VerbTaskResult()
                {
                    ExitCode = 1,
                    RaisedExceptions = e.InnerExceptions,
                };
            }
        }
        public abstract Task<int> Execute(T arguments, string[] args);
    }
}
