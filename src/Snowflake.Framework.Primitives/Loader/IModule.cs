using System.IO;

namespace Snowflake.Loader
{
    public interface IModule
    {
        string Author { get; }
        string Entry { get; }
        string Loader { get; }
        DirectoryInfo ModuleDirectory { get; }
        string Name { get; }
    }
}