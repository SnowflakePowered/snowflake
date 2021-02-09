using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Filesystem
{
    public interface IProjectingDirectory
        : IMutableDirectoryBase,
          IFileOpeningDirectoryBase<IFile>,
        IDirectoryOpeningDirectoryBase<IProjectingDirectory>,
        IEnumerableDirectoryBase<IReadOnlyDirectory, IReadOnlyFile>
    {
        IReadOnlyFile Project(IFile file);
        IReadOnlyFile Project(IFile file, string name);
        IReadOnlyDirectory Project(IDirectory directory);
        IReadOnlyDirectory Project(IDirectory directory, string name);

        IReadOnlyFile Project(IReadOnlyFile file);
        IReadOnlyFile Project(IReadOnlyFile file, string name);
        IReadOnlyDirectory Project(IReadOnlyDirectory directory);
        IReadOnlyDirectory Project(IReadOnlyDirectory directory, string name);

        /// <summary>
        /// Deletes the directory, including all files and subdirectories included.
        /// 
        /// This will invalidate all instances of <see cref="IDirectory"/> pointing to
        /// this specific directory until it exists again. 
        /// </summary>       
        /// <exception cref="IOException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        void Delete();
    }
}
