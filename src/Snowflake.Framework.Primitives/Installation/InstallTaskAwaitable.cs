using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Snowflake.Installation.TaskResult;

namespace Snowflake.Installation
{
    public abstract class InstallTaskAwaitable<T>
    {
        private Task<T> BaseTask => this.Execute();

        protected abstract Task<T> Execute();

        protected abstract string TaskName { get; }

        public ConfiguredTaskAwaitable<TaskResult<T>>.ConfiguredTaskAwaiter GetAwaiter()
        {
            return this.BaseTask
                .ContinueWith(f => f.Exception == null ?
                     Success(this.TaskName, f) : Failure(this.TaskName, f, f.Exception))
                .ConfigureAwait(false)
                .GetAwaiter();
        }
    }
}
