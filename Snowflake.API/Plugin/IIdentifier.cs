using System.ComponentModel.Composition;
using System.IO;

namespace Snowflake.Plugin
{
    [InheritedExport(typeof(IIdentifier))]
    public interface IIdentifier : IPlugin
    {
        string IdentifyGame(string fileName, string platformId);

        string IdentifyGame(FileStream file, string platformId);
    }
}
