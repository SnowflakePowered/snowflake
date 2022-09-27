using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Filesystem.Library
{
    public interface IContentLibraryStore
    {
        public IEnumerable<IContentLibrary> GetLibraries();
    }
}
