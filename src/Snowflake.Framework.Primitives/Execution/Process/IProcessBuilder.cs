using System.Diagnostics;

namespace Snowflake.Execution.Process
{
    public interface IProcessBuilder
    {
        ProcessStartInfo ToProcessStartInfo();
        IProcessBuilder WithArgument(string switchName);
        IProcessBuilder WithArgument(string parameterName, string value, bool quoted = true);
    }
}