using System;
using System.Collections.Generic;

namespace Snowflake.Remoting.Marshalling
{
    public interface IArgumentTypeMapper
    {
        void Add<T>(Func<string, T> converter);
        IEnumerable<ITypedArgument> CastArguments(IEnumerable<ISerializedArgument> pathArgs,
                IEnumerable<ISerializedArgument> endpointArgs);
        object ConvertValue(Type targetType, string value);
        T ConvertValue<T>(string value);
    }
}