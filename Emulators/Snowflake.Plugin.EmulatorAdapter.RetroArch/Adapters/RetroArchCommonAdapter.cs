using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Input;
using Snowflake.Extensibility;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters
{
    internal abstract class RetroArchCommonAdapter : Extensibility.Plugin
    {
        public IEnumerable<IInputMapping> InputMappings { get; }
        protected RetroArchCommonAdapter(string appDataDirectory) : base(appDataDirectory)
        {
            this.GetAllSiblingResourceNames("RetroArchCommon");
        }
    }
}
