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
        public static IDirectory AsIndelible(this IMutableDirectoryBase<IDeletableDirectory, IDirectory> @this) 
            => @this.ReopenAs();

        /// <summary>
        /// Returns an undeletable version of this directory
        /// </summary>
        /// <returns></returns>
        public static IDirectory AsIndelible(this IMutableDirectoryBase<IMoveFromableDirectory, IDirectory> @this)
            => @this.ReopenAs();

        /// <summary>
        /// Returns an undeletable version of this directory
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyDirectory AsReadOnly(this IMutableDirectoryBase<IDeletableDirectory, IReadOnlyDirectory> @this)
            => @this.ReopenAs();

        /// <summary>
        /// Returns a version of this directory that can have files moved into it.
        /// </summary>
        /// <returns></returns>
        public static IMoveFromableDirectory AsMoveFromable(this IMutableDirectoryBase<IDeletableDirectory, IMoveFromableDirectory> @this) =>
            @this.ReopenAs();

        /// <summary>
        /// Returns a version of this directory that can have files moved into it.
        /// </summary>
        /// <returns></returns>
        public static IDeletableMoveFromableDirectory AsMoveFromable(this IDeletableDirectory @this) =>
            ((IMutableDirectoryBase<IDeletableDirectory, IDeletableMoveFromableDirectory>)@this).ReopenAs();
    }
}
#pragma warning restore CS0618
