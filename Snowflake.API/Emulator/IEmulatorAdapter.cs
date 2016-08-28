using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Hotkey;
using Snowflake.Configuration.Input;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Configuration;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Service;

namespace Snowflake.Emulator
{
    public interface IEmulatorAdapter : IPlugin
    {
        IEnumerable<IInputMapping> InputMappings { get; }
        IEnumerable<string> Capabilities { get; }
        IEnumerable<string> Mimetypes { get; }
        ISaveManager SaveManager { get; }
        IBiosManager BiosManager { get; }
        string SaveType { get; }
        IEmulatorInstance Instantiate(IGameRecord gameRecord, IFileRecord romFile, int saveSlot, IList<IEmulatedPort> ports);
        IDictionary<string, IConfigurationCollection> GetConfigurations(IGameRecord gameRecord);
        IHotkeyTemplate GetHotkeyTemplate();
        IEmulatorInstance Instantiate(IGameRecord gameRecord, ICoreService coreService);
    }
}
