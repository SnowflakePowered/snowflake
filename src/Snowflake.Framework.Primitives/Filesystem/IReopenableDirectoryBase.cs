using System;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Base interface for a reopenable  directory.
    /// 
    /// The most basic directory type is <see cref="IDirectory"/>
    /// </summary>
    /// <typeparam name="TReopenableAs">The type of directory this directory can be reopenable as</typeparam>
    public interface IReopenableDirectoryBase<TReopenableAs>
    {
        /// <summary>
        /// Reopens the directory with the implemented capability.
        /// 
        /// This is an implementation detail, and if used incorrectly, will cause issues. 
        /// Instead, use the extension methods in <see cref="DirectoryConversionExtensions"/>, such as
        /// <see cref="DirectoryConversionExtensions.AsReadOnly(IReopenableDirectoryBase{IReadOnlyDirectory})"/>
        /// or <see cref="DirectoryConversionExtensions.AsIndelible(IReopenableDirectoryBase{IDirectory})"/>.
        /// </summary>
        /// <returns>The directory with the implemented capability.</returns>
        [Obsolete("Internal implementation detail. Use As* methods instead.")]
        TReopenableAs ReopenAs();
    }
}
