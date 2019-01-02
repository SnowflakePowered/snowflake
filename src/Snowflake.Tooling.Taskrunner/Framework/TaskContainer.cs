using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Snowflake.Tooling.Taskrunner.Framework.Tasks;

namespace Snowflake.Tooling.Taskrunner.Framework
{
    public class TaskContainer : IEnumerable<ITaskRunner>
    {
        private Dictionary<string, ITaskRunner> Container { get; }

        public ITaskRunner this[string key] => this.Container[key];

        public TaskContainer()
        {
            this.Container = new Dictionary<string, ITaskRunner>();
        }

        public void Add(ITaskRunner task)
        {
            this.Container.Add(task.Name, task);
        }

        public bool Contains(string taskName)
        {
            return this.Container.ContainsKey(taskName);
        }

        public IEnumerator<ITaskRunner> GetEnumerator()
        {
            return this.Container.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
