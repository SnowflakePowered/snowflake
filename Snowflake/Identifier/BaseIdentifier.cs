using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Snowflake.Plugin;
using Snowflake.Service;
using Snowflake.Platform;
namespace Snowflake.Identifier
{
    public abstract class BaseIdentifier : BasePlugin, IIdentifier
    {
        protected BaseIdentifier(Assembly pluginAssembly, ICoreService coreInstance)
            : base(pluginAssembly, coreInstance)
        {
            if(this.PluginInfo.ContainsKey("identifier_valuetype")){
                this.IdentifiedValueType = this.PluginInfo["identifier_valuetype"];
            }else{
                this.IdentifiedValueType = IdentifiedValueTypes.Unknown;
            }
        }
        public string IdentifiedValueType { get; private set; }
        public abstract string IdentifyGame(string fileName, string platformId);
        public abstract string IdentifyGame(FileStream file, string platformId);

    }
}
