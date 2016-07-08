using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Extensibility;
using Snowflake.Records.Game;

namespace Snowflake.Emulator
{
    public interface IEmulatorAdapter 
    {
        IEmulatorInstance Instantiate(IGameRecord record, IList<IConfigurationCollection> collection);
        IList<string> SupportedMimetypes { get; }
        string PluginName { get; }
    }
}
