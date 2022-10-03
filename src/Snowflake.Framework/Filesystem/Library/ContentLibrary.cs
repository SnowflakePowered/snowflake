using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zio;
using Zio.FileSystems;

namespace Snowflake.Filesystem.Library
{
    internal class ContentLibrary : IContentLibrary
    {
        public ContentLibrary(Guid libraryId, IDirectory rootDirectory)
        {
            this.LibraryID = libraryId;
            this.Root = rootDirectory;
        }

        public Guid LibraryID { get; }

#pragma warning disable CS0618 // Type or member is obsolete
        public DirectoryInfo Path => this.Root.UnsafeGetPath();
#pragma warning restore CS0618 // Type or member is obsolete

        private IDirectory Root { get; }

        public IDirectory OpenRecordLibrary(Guid recordId)
        {
            return this.Root.OpenDirectory(recordId.ToString()).AsIndelible();
        }
    }
}
