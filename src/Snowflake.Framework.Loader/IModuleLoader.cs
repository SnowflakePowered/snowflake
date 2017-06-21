using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Loader
{
    public interface IModuleLoader<out T> 
    {
        IEnumerable<T> LoadModule(Module module);
    }
}
