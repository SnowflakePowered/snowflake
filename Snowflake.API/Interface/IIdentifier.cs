using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.Composition;

namespace Snowflake.API.Interface
{
    [InheritedExport(typeof(IIdentifier))]
    public interface IIdentifier : IPlugin
    {
        string IdentifyGame(string fileName, string platformId);

        string IdentifyGame(FileStream file, string platformId);
    }
}
