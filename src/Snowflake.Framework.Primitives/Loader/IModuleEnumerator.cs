using System.Collections.Generic;

namespace Snowflake.Loader
{
    /// <summary>
    /// Enumerates all modules installed in the folder.
    /// </summary>
    public interface IModuleEnumerator
    {
        /// <summary>
        /// Gets all the modules installed in the folder.
        /// </summary>
        IEnumerable<IModule> Modules { get; }
    }
}
