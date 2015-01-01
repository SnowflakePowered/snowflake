using System.ComponentModel.Composition;
using System.IO;
using Snowflake.Plugin;

namespace Snowflake.Identifier
{
    [InheritedExport(typeof(IIdentifier))]
    public interface IIdentifier : IBasePlugin
    {
        string IdentifyGame(string fileName, string platformId);
        string IdentifyGame(FileStream file, string platformId);
    }
}
