using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Filesystem
{
    public interface IProjectingDirectory
        : IMutableDirectoryBase<IProjectingDirectory>,
        IMutableDirectoryBase<IProjectingDirectory, IReadOnlyDirectory, IReadOnlyFile>
    {
        IReadOnlyFile Project(IFile file);
        IReadOnlyFile Project(IFile file, string name);
        IReadOnlyDirectory Project(IDirectory directory);
        IReadOnlyDirectory Project(IDirectory directory, string name);

        IReadOnlyFile Project(IReadOnlyFile file);
        IReadOnlyFile Project(IReadOnlyFile file, string name);
        IReadOnlyDirectory Project(IReadOnlyDirectory directory);
        IReadOnlyDirectory Project(IReadOnlyDirectory directory, string name);

        void Delete();
    }
}
