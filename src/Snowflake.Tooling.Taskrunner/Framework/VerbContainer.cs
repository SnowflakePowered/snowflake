using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Snowflake.Tooling.Taskrunner.Framework.Tasks;

namespace Snowflake.Tooling.Taskrunner.Framework
{
    public class VerbContainer : IEnumerable<IVerbTask>
    {
        private Dictionary<string, IVerbTask> Container { get; }

        public IVerbTask this[string key] => this.Container[key];

        public VerbContainer()
        {
            this.Container = new Dictionary<string, IVerbTask>();
        }

        public void Add (IVerbTask task)
        {
            this.Container.Add(task.Name, task);
        }

        public bool Contains(string taskName)
        {
            return this.Container.ContainsKey(taskName);
        }

        public IEnumerator<IVerbTask> GetEnumerator()
        {
            return this.Container.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
