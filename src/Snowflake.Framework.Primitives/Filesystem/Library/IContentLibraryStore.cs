using Snowflake.Model.Records;
using Snowflake.Model.Records.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Filesystem.Library
{
    public interface IContentLibraryStore
    {
        IContentLibrary CreateLibrary(DirectoryInfo dirInfo);
        IContentLibrary? GetRecordLibrary(IRecord record);
        IEnumerable<IContentLibrary> GetLibraries();
        IContentLibrary? GetLibrary(Guid libraryId);
        void SetRecordLibrary(IContentLibrary library, IRecord record);
    }
}
