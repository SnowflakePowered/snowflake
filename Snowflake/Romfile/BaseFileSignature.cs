using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Plugin.Configuration;
using Snowflake.Service;
using Snowflake.Plugin;
namespace Snowflake.Romfile
{
    public abstract class BaseFileSignature : BasePlugin, IFileSignature
    {
        public abstract string FileExtension { get; }
        public abstract long HeaderOffset { get; }
        public abstract byte[] HeaderSignature { get; }
        public abstract string StonePlatformId { get; }

        public abstract bool FileExtensionMatches(string fileName);
        public abstract string GetGameID(string fileName);
        public abstract bool HeaderSignatureMatches(string fileName);

    }
}
