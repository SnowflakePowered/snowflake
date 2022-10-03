using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Filesystem.Library
{
    public interface IContentLibrary
    {
        public Guid LibraryID { get; }
        public DirectoryInfo Path { get; }
        public IDirectory OpenRecordLibrary(Guid recordId);
    }
}
