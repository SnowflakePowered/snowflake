using System;

namespace Snowflake.Remoting.Resources
{
    public interface IParameter
    {
        string Key { get; }
        Type Type { get; }
    }
}