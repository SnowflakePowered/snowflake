using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Filesystem.Library
{
    public interface IContentLibrary
    {
        public Guid LibraryID { get; }

        public IDirectory OpenLibrary(IGame game);
    }
}
