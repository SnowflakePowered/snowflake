using System.IO;
using System.Reflection;
using Snowflake.Plugin;
using Snowflake.Service;

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
        public string IdentifiedValueType { get; }
        public abstract string IdentifyGame(string fileName, string platformId);
        public abstract string IdentifyGame(FileStream file, string platformId);

    }
}
