using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Installation.Tasks
{
    /// <summary>
    /// Wraps a <see cref="Task{TResult}"/> into an <see cref="AsyncInstallTask{TResult}"/>
    /// that yields <see cref="TaskResult{TResult}"/>.
    /// </summary>
    /// <typeparam name="TResult">The return type of the task.</typeparam>
    public class ExecuteAsyncTask<TResult>
        : AsyncInstallTask<TResult>
    {
        /// <summary>
        /// Describes the execution result of the supplied task as a <see cref="TaskResult{T}"/>
        /// </summary>
        /// <param name="task">The task to execute.</param>
        public ExecuteAsyncTask(Task<TResult> task)
        {
            this.Task = task;
        }

        private Task<TResult> Task { get; }

        protected override string TaskName => $"Execute{typeof(TResult).Name}Result";

        protected override async Task<TResult> ExecuteOnce()
        {
            return await this.Task;
        }
    }
}
