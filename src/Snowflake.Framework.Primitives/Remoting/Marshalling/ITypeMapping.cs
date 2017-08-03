using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.Marshalling
{
    public interface ITypeMapping<T>
    {
        T ConvertValue(string value);
    }
}
