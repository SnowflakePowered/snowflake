using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tooling.Taskrunner.Framework.Tasks
{
    public sealed class VerbTaskResult
    {
        public int ExitCode { get; set; } = 0;
        public IEnumerable<Exception> RaisedExceptions { get; set; } = Enumerable.Empty<Exception>();

        public static implicit operator VerbTaskResult(int exitCode)
        {
            return new VerbTaskResult() {ExitCode = exitCode};
        }
    }
}
