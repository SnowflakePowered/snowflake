namespace Snowflake.Filesystem
{
    /// <summary>
    /// Base interface for a mutable directory.
    /// 
    /// Never refer to this interface directly. The most basic directory type is <see cref="IDirectory"/>
    /// </summary>
    /// <typeparam name="TChildDirectory">The type of child directory this directory will open</typeparam>
    public interface IDirectoryOpeningDirectoryBase<TChildDirectory>
    {
        /// <summary>
        /// Opens an existing descendant directory with the given name.
        /// If the directory does not exist, creates the directory.
        /// You can open a nested directory using '/' as the path separator, and it 
        /// will be created relative to this current directory.
        /// </summary>
        /// <param name="name">The name of the existing directory</param>
        /// <returns>The directory if it exists, or null if it does not.</returns>
        TChildDirectory OpenDirectory(string name);
    }
}
