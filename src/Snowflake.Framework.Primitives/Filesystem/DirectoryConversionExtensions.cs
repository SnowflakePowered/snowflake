using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Provides conversions for directory types.
    /// </summary>
#pragma warning disable CS0618
    public static class DirectoryConversionExtensions
    {
        /// <summary>
        /// Returns an undeletable version of this directory
        /// </summary>
        /// <returns></returns>
        public static IDirectory AsIndelible(this IReopenableDirectoryBase<IDirectory> @this) 
            => @this.ReopenAs();

        /// <summary>
        /// Returns an undeletable version of this directory
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyDirectory AsReadOnly(this IReopenableDirectoryBase<IReadOnlyDirectory> @this)
            => @this.ReopenAs();

        /// <summary>
        /// Returns a version of this directory that can have files moved into it.
        /// </summary>
        /// <returns></returns>
        public static IMoveFromableDirectory AsMoveFromable(this IReopenableDirectoryBase<IMoveFromableDirectory> @this) =>
            @this.ReopenAs();

        /// <summary>
        /// Returns a version of this directory that can have files moved into it.
        /// </summary>
        /// <returns></returns>
        public static IDeletableMoveFromableDirectory AsMoveFromable(this IDeletableDirectory @this) =>
            ((IReopenableDirectoryBase<IDeletableMoveFromableDirectory>)@this).ReopenAs();

        /// <summary>
        /// Returns an disposable version of this directory.
        /// </summary>
        /// <exception cref="System.IO.IOException">When the directory is not empty.</exception>
        /// <returns></returns>
        public static IDisposableDirectory AsDisposable(this IReopenableDirectoryBase<IDisposableDirectory> @this)
            => @this.ReopenAs();
    }
}
#pragma warning restore CS0618
