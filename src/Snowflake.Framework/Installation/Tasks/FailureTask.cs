using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Installation.Tasks
{
    /// <summary>
    /// Immediately fails the task by throwing an exception.
    /// </summary>
    public sealed class FailureTask<T> : AsyncInstallTask<T>
    {
        /// <summary>
        /// Fails with the provided exception
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        public FailureTask(Exception exception)
        {
            this.Exception = exception;
        }

        /// <summary>
        /// Fails with the provided message using a generic exception.
        /// </summary>
        /// <param name="message">The messsage to fail with.</param>
        public FailureTask(string message)
        {
            this.Exception = new Exception(message);
        }

        /// <summary>
        /// Fails with the provided message using the provided exception.
        /// </summary>
        /// <param name="message">The messsage to fail with.</param>
        /// <param name="innerException">The inner exception.</param>
        public FailureTask(string message, Exception innerException)
        {
            this.Exception = new Exception(message, innerException);
        }

        private Exception Exception { get; }

        protected override string TaskName => "Failure";

        protected override ValueTask<string> CreateFailureDescription(AggregateException e)
        {
            if (this.Exception.InnerException != null) return new ValueTask<string>($"{this.Exception.Message} ({this.Exception.InnerException.Message})");
            return new ValueTask<string>($"{this.Exception.Message}");
        }

        protected override Task<T> ExecuteOnce()
        {
            return Task.FromException<T>(this.Exception);
        }
    }
}
