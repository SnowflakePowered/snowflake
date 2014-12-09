using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Snowflake.Emulator.Configuration.Mapping;

using Snowflake.Emulator.Configuration.Template;
namespace Snowflake.Emulator.Configuration
{
    public interface IEmulatorConfiguration
    {
        BooleanMapping BooleanMapping { get;}
        string FileName { get; }
        ConfigurationTemplate ConfigurationTemplate { get; }
    }
}
