using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Constants.Plugin;
using Snowflake.Plugin.Configuration;
using Snowflake.Service;
using Snowflake.Plugin;
namespace Snowflake.Romfile
{
    public abstract class FileSignature : BasePlugin, IFileSignature
    {

        public IList<string> FileExtensions { get; }
        public string SupportedPlatform { get; }

        public abstract long HeaderOffset { get; }
        public abstract byte[] HeaderSignature { get; }

        public abstract bool FileExtensionMatches(string fileName);
        public abstract string GetGameId(string fileName);
        public abstract bool HeaderSignatureMatches(string fileName);

        protected FileSignature(Assembly pluginAssembly, ICoreService coreInstance) : base(pluginAssembly, coreInstance) {
            this.FileExtensions = this.PluginInfo["file_extensions"].ToObject<IList<string>>();
            this.SupportedPlatform = this.SupportedPlatforms.First();
        }
    }
}
