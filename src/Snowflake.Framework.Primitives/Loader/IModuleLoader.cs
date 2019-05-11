using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Loader
{
    /// <summary>
    /// Implements a loader for modules of  type<typeparamref name="T"/> from a module specification.
    /// </summary>
    /// <typeparam name="T">The type of the object that this module loader resolves from a module specification.</typeparam>
    public interface IModuleLoader<out T>
    {
        /// <summary>
        /// Loads a module from its specification and return the resolved representations.
        /// </summary>
        /// <param name="module">The module specification.</param>
        /// <returns>An enumerable of resolved module representations. One specification can result in multiple
        /// objects being resolved.</returns>
        IEnumerable<T> LoadModule(IModule module);
    }
}
