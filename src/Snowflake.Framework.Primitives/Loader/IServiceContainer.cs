using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Loader
{
    public interface IServiceContainer
    {
        T Get<T>();
        IEnumerable<string> Services { get; }
    }
}
