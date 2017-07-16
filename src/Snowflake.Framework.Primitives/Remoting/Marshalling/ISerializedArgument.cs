using System;

namespace Snowflake.Remoting.Marshalling
{
    public interface ISerializedArgument
    {
        string Key { get; }
        string StringValue { get; }
        Type Type { get; }
    }
}