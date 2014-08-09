using System.ComponentModel.Composition;
using Snowflake.Plugin.Interface;

namespace Snowflake.Emulator
{
    [InheritedExport(typeof(IEmulator))]
    public interface IEmulator : IPlugin
    {
        void Run(string gameUuid);
    }
}
