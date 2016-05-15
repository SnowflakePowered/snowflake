using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Constants.Plugin;
using Snowflake.Extensibility.Configuration;
using Snowflake.Service;
using Snowflake.Extensibility;
namespace Snowflake.Romfile
{
    public abstract class FileSignature : Extensibility.Plugin, IFileSignature
    {

        public IList<string> FileTypes { get; }
        public string SupportedPlatform { get; }


        public abstract byte[] HeaderSignature { get; }

        public virtual string GetGameId(Stream fileContents)
        {
            return "";
        }
        public virtual string GetInternalName(Stream fileContents)
        {
            return "";
        }

        public abstract bool HeaderSignatureMatches(Stream fileContents);

        protected FileSignature(ICoreService coreInstance) : base(coreInstance) {
            this.FileTypes = this.PluginProperties.GetEnumerable("file_filetype").ToList();
            this.SupportedPlatform = this.SupportedPlatforms.First();
        }

    }
}
