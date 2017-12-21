using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Snowflake.Execution.Process
{
    internal class ProcessBuilder : IProcessBuilder
    {
        private FileInfo ProcessPath { get; }
        private IList<string> Arguments { get; }
        public ProcessBuilder(FileInfo processPath)
        {
            this.ProcessPath = processPath;
            this.Arguments = new List<string>();
        }

        public IProcessBuilder WithArgument(string switchName)
        {
            this.Arguments.Add(switchName);
            return this;
        }

        public IProcessBuilder WithArgument(string parameterName, string value, bool quoted = true)
        {
            if (quoted)
            {
                this.Arguments.Add($@"{parameterName} ""{EscapedArgument(value)}""");
            }
            else
            {
                this.Arguments.Add($"{parameterName} {EscapedArgument(value)}");
            }

            return this;
        }

        private static string EscapedArgument(string arg) => arg.Replace(@"\", @"\\");

        public ProcessStartInfo ToProcessStartInfo()
        {
            var psi = new ProcessStartInfo()
            {
                FileName = this.ProcessPath.FullName,
                Arguments = string.Join(" ", this.Arguments),
            };

            return psi;
        }
    }
}
