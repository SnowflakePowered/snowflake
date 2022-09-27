using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zio.FileSystems;

namespace Snowflake.Filesystem.Library
{
    internal class ContentLibrary : IContentLibrary
    {
        public ContentLibrary(Guid libraryId, SubFileSystem subFs)
        {
            this.LibraryID = libraryId;
        }

        public Guid LibraryID { get; }

        public IDirectory OpenLibrary(IGame game)
        {
            throw new NotImplementedException();
        }
    }
}
