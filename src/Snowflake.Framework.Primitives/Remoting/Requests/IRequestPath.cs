using System.Collections.Immutable;

namespace Snowflake.Remoting.Requests
{
    public interface IRequestPath
    {
        IImmutableList<string> PathNodes { get; }
    }
}