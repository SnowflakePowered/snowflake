using System.Collections.Generic;

namespace Snowflake.Loader
{
    public interface IModuleEnumerator
    {
        IEnumerable<IModule> Modules { get; }
    }
}