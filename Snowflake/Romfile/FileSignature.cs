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

        public abstract byte[] HeaderSignature { get; }

        public virtual string GetGameId(string fileName)
        {
            return "";
        }
        public virtual string GetInternalName(string fileName)
        {
            return "";
        }
        public abstract bool HeaderSignatureMatches(string fileName);

        protected FileSignature(Assembly pluginAssembly, ICoreService coreInstance) : base(pluginAssembly, coreInstance) {
            this.FileExtensions = this.PluginInfo["file_extensions"].ToObject<IList<string>>();
            this.SupportedPlatform = this.SupportedPlatforms.First();
        }

        public virtual bool FileExtensionMatches(string fileName)
        {
            return this.FileExtensions.Contains(Path.GetExtension(fileName));
        }

    }
}
