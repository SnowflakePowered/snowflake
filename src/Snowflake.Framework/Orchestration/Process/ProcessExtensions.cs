using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Process
{
    public static class ProcessExtensions
    {
        /// <summary>
        /// Waits asynchronously for the process to exit.
        /// 
        /// </summary>
        /// <param name="process">The process to wait for cancellation.</param>
        /// <param name="cancellationToken">A cancellation token. If invoked, the task will return 
        /// immediately as canceled.</param>
        /// <returns>A Task representing waiting for the process to end.</returns>
        /// https://stackoverflow.com/questions/36545858/process-waitforexitint32-asynchronously
        public static Task WaitForExitAsync(this System.Diagnostics.Process process,
            CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(null!);
            if (cancellationToken != default) cancellationToken.Register(tcs.SetCanceled);

            return tcs.Task;
        }
    }
}
