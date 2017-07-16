using System;

namespace Snowflake.Remoting.Marshalling
{
    public interface ITypedArgument
    {
        string Key { get; }
        Type Type { get; }
        object Value { get; }
    }
}