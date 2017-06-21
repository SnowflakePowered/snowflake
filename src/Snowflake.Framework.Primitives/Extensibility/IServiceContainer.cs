using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Extensibility
{
    public interface IServiceContainer
    {
        T Get<T>();
        IEnumerable<string> Services { get; }
        string AppDataDirectory { get; }
    }
}
