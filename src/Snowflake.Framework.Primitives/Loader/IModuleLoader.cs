using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Loader
{
    public interface IModuleLoader<out T>
    {
        IEnumerable<T> LoadModule(IModule module);
    }
}
